using System.Linq.Expressions;
using Comments.DAL.Entities;
using Comments.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Comments.DAL.Repositories.Implementations
{ 
    public class CommentsRepository : ICommentsRepository
    {
        private readonly IMongoCollection<Comment> _comments;
        private readonly MongoDbSettings _settings;
        private readonly ICacheRepository _cacheRepository;

        public CommentsRepository(
            IOptions<MongoDbSettings> settings,
            IOptions<CacheSettings> cacheSettings,
            ICacheRepository cacheRepository)
        {
            _settings = settings.Value;

            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _comments = database.GetCollection<Comment>(_settings.CommentsCollectionName);

            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>>? expression = null)
        {
            var cachedComments = await _cacheRepository
                .GetDataAsync<IEnumerable<Comment>>(CacheKeys.CommentsCacheKey);

            if (cachedComments is null)
            {
                var comments = await _comments.Find(expression is null ?
                    s => true : 
                    expression).ToListAsync();

                await _cacheRepository.SetDataAsync(CacheKeys.CommentsCacheKey, comments, DateTimeOffset.Now.AddMinutes(1));
                
                return comments;
            }
            
            if (expression is null)
            {
                return cachedComments;
            }
            else
            {
                return cachedComments.Where(expression.Compile());
            }
        }

        public async Task<Comment?> GetByConditionAsync(Expression<Func<Comment, bool>> expression)
        {
            var cachedComments = await _cacheRepository
                .GetDataAsync<IEnumerable<Comment>>(CacheKeys.CommentsCacheKey);

            if (cachedComments is null)
            {
                return await _comments.Find(expression).FirstOrDefaultAsync();
            }
            
            return cachedComments.FirstOrDefault(expression.Compile());
        }

        public async Task AddAsync(Comment comment)
        {
            await _comments.InsertOneAsync(comment);

            var expirationTime = DateTimeOffset.UtcNow.AddMinutes(1);
            await _cacheRepository.SetDataAsync(CacheKeys.CommentsCacheKey, comment, expirationTime);
        }

        public async Task DeleteAsync(string id)
        {
            await _comments.DeleteOneAsync(x =>x.Id == id);

            await _cacheRepository.RemoveDataAsync(id);
        }

        public async Task UpdateAsync(Comment comment)
        {
            await _comments.ReplaceOneAsync(x => x.Id == comment.Id, comment);

            var expirationTime = DateTimeOffset.UtcNow.AddMinutes(1);
            await _cacheRepository.SetDataAsync(CacheKeys.CommentsCacheKey, comment, expirationTime);
        }
    }
}
