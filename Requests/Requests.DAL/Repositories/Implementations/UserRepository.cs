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
        private readonly IOptions<MongoDbSettings> _settings;

        public UserRepository(IOptions<MongoDbSettings> settings)
        {
            _settings = settings;
            var client = new MongoClient(_settings.Value.ConnectionString);
            var database = client.GetDatabase(_settings.Value.DatabaseName);
            _users = database.GetCollection<User>(_settings.Value.UsersCollectionName);
        }

        public async Task<User> GetByConditionAsync(Expression<Func<User, bool>> expression) =>
            await _users.Find(expression).FirstOrDefaultAsync();
    }
}
