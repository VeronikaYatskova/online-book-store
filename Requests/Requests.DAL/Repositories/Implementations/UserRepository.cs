using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Requests.DAL.Models;
using Requests.DAL.Repositories.Interfaces;

namespace Requests.DAL.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoDbSettings _settings;

        public UserRepository(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _users = database.GetCollection<User>(_settings.UsersCollectionName);
        }

        public async Task<User> GetByConditionAsync(Expression<Func<User, bool>> expression) =>
            await _users.Find(expression).FirstOrDefaultAsync();

        public async Task AddUserAsync(User user) =>
            await _users.InsertOneAsync(user);
    }
}
