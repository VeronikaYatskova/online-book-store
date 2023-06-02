using Auth.Application.DTOs.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.Features.User.Queries.GetUsers;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Application.Features.User.Commands.LoginUser;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration config;

        public UserController(IMediator mediator, IConfiguration config)
        {
            this.mediator = mediator;
            this.config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await mediator.Send(new GetUsersQuery());

            return Ok(users);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var key = config["SecretKey"];
            var token = await mediator.Send(new LoginUserCommand(request, key!));

            return Ok(token);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var key = config["SecretKey"];
            var token = await mediator.Send(new RegisterUserCommand(request, key!));

            return Created("User was created.", token);
        }
    }
}
