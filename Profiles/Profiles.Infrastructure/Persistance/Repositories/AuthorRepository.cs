using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public AuthorRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<IEnumerable<User>> GetAuthorsAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='984a871c-e075-4fea-84d1-672dc4212b32'";
            var authors = await connection.QueryAsync<User>(query);

            return authors;
        }
    }
}
