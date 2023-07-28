using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public UserRepository(IConfiguration config, IOptions<DatabaseSettings> databaseSettings)
        {
            _config = config;
            _databaseSettings = databaseSettings;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users";
            var users = await connection.QueryAsync<User>(query);

            return users;
        }

        public async Task<IEnumerable<User>> GetAuthorsAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='984a871c-e075-4fea-84d1-672dc4212b32'";
            var authors = await connection.QueryAsync<User>(query);

            return authors;
        }

        public async Task<IEnumerable<User>> GetNormalUsersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='09f1a17e-a830-415d-8611-7c9595d3dcc5'";
            var authors = await connection.QueryAsync<User>(query);

            return authors;
        }

        public async Task<IEnumerable<User>> GetPublishersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='4e27e0eb-2033-4db8-85a4-86c40f8122f7'";
            var publishers = await connection.QueryAsync<User>(query);

            return publishers;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE Id = @id";
            var user = await connection.QuerySingleOrDefaultAsync<User>(query, userId);

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

            await connection.ExecuteAsync(query, userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "UPDATE Users SET Email=@Email, FirstName=@FirstName, LastName=@LastName WHERE Id=@Id";

            await connection.ExecuteAsync(query, user);
        }
    }
}
