using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration config;
        private readonly string connectionString;

        public UserRepository(IConfiguration config)
        {
            this.config = config;
            connectionString = config.GetConnectionString("MsSqlConnection");
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(connectionString);
            var query = "SELECT * FROM Users";
            var users = await connection.QueryAsync<User>(query);

            return users;
        }

        public Task<IEnumerable<User>> GetAuthorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetNormalUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetPublishersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(connectionString);
            var query = "SELECT * FROM Users WHERE Id = @id";
            var user = await connection.QuerySingleOrDefaultAsync<User>(query, userId);

            return user;
        }

        public async Task AddUserAsync(User user)
        {
            using var connection = new SqlConnection(config.GetConnectionString("MsSqlConnection"));
            var query = "INSERT INTO USERS (Id, Email, FirstName, LastName) VALUES (NEWID(), @Email, @FirstName, @LastName)";

            await connection.ExecuteAsync(query, user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            using var connection = new SqlConnection(config.GetConnectionString("MsSqlConnection"));
            var query = "DELETE FROM Users WHERE Id=@Id";

            await connection.ExecuteAsync(query, userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = new SqlConnection(config.GetConnectionString("MsSqlConnection"));
            var query = "UPDATE Users SET Email=@Email, FirstName=@FirstName, LastName=@LastName WHERE Id=@Id";

            await connection.ExecuteAsync(query, user);
        }
    }
}
