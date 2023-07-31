using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Profiles.Application.Interfaces.Repositories;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistance.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly IOptions<DatabaseSettings> _databaseSettings;

        public PublisherRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public async Task<IEnumerable<User>> GetPublishersAsync()
        {
            using var connection = new SqlConnection(_databaseSettings.Value.MsSqlConnectionString);
            var query = "SELECT * FROM Users WHERE RoleId='4e27e0eb-2033-4db8-85a4-86c40f8122f7'";
            var publishers = await connection.QueryAsync<User>(query);

            return publishers;
        }
    }
}
