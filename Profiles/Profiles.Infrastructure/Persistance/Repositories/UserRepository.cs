using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public UserRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users";
            var users = await connection.QueryAsync<User>(query);

            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE Id = @userId";
            var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { userId });

            return user;
        }

        public async Task AddUserAsync(User user)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "INSERT INTO USERS (Id, Email, FirstName, LastName, RoleId) VALUES (NEWID(), @Email, @FirstName, @LastName, @RoleId)";

            await connection.ExecuteAsync(query, user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "DELETE FROM Users WHERE Id=@Id";

            await connection.ExecuteAsync(query, new { userId });
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "UPDATE Users SET Email=@Email, FirstName=@FirstName, LastName=@LastName WHERE Id=@Id";

            await connection.ExecuteAsync(query, user);
        }
    }
}
