using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Profiles.Domain.Entities;

namespace Profiles.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration config;

        public UsersController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            using var connection = new SqlConnection(config.GetConnectionString("MsSqlConnection"));
            var users = await connection.QueryAsync<User>("SELECT * FROM Users");

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            using var connection = new SqlConnection(config.GetConnectionString("MsSqlConnection"));
            
            await connection.ExecuteAsync("INSERT INTO USERS (Id, Email, FirstName, LastName)" + 
            "VALUES (NEWID(), @Email, @FirstName, @LastName)", user);

            return Created("User is created", user);
        }
    }
}
