using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.DAL.Repositories.Implementations
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly IMongoCollection<Request> _requests;
        private readonly MongoDbSettings _settings;

        public RequestsRepository(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _requests = database.GetCollection<Request>(_settings.RequestsCollectionName);
        }

        public async Task<IEnumerable<Request>> GetAllAsync(Expression<Func<Request, bool>>? expression = null) =>
            expression is null ?
                await _requests.Find(s => true).ToListAsync() : 
                await _requests.Find(expression).ToListAsync();
       
        public async Task<Request> GetByConditionAsync(Expression<Func<Request, bool>> expression) =>
            await _requests.Find(expression).FirstOrDefaultAsync();

        public async Task UpdateAsync(Request request) =>
            await _requests.ReplaceOneAsync(x => x.Id == request.Id, request);
    
        public async Task AddAsync(Request request) =>
            await _requests.InsertOneAsync(request);        

        public async Task DeleteAsync(string id) =>
            await _requests.DeleteOneAsync(x =>x.Id == id);
    }
}
