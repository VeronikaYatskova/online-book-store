using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class NormalUserRepository : INormalUserRepository
    {
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public NormalUserRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }
        
        public async Task<IEnumerable<User>> GetNormalUsersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='09f1a17e-a830-415d-8611-7c9595d3dcc5'";
            var users = await connection.QueryAsync<User>(query);

            return users;
        }
    }
}
