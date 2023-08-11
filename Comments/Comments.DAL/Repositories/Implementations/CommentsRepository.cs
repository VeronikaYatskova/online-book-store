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
        private readonly IOptions<MongoDbSettings> _settings;

        public CommentsRepository(IOptions<MongoDbSettings> settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.Value.ConnectionString);
            var database = client.GetDatabase(_settings.Value.DatabaseName);
            _comments = database.GetCollection<Comment>(_settings.Value.CommentsCollectionName);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(Expression<Func<Comment, bool>>? expression = null) =>
            expression is null ?
                await _comments.Find(s => true).ToListAsync() : 
                await _comments.Find(expression).ToListAsync(); 

        public async Task<Comment> GetByConditionAsync(Expression<Func<Comment, bool>> expression) =>
            await _comments.Find(expression).FirstOrDefaultAsync();

        public async Task AddAsync(Comment comment) =>
            await _comments.InsertOneAsync(comment);        
        
        public async Task DeleteAsync(string id) =>
            await _comments.DeleteOneAsync(x =>x.Id == id);

        public async Task UpdateAsync(Comment comment) =>
            await _comments.ReplaceOneAsync(x => x.Id == comment.Id, comment);
    }
}
