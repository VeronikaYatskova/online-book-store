using Auth.Application.DTOs.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.Features.User.Queries.GetUsers;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Application.Features.User.Commands.LoginUser;
using Auth.Application.Features.User.Commands.GetRefreshToken;
using Auth.Application.Features.User.Queries.GetRedirectUrl;
using Auth.Application.Features.User.Commands.LoginUserViaGoogle;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await mediator.Send(new GetUsersQuery());

            return Ok(users);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var token = await mediator.Send(new LoginUserCommand(request));

            return Ok(token);
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var token = await mediator.Send(new RegisterUserCommand(request));

            return Created("User was created.", token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await mediator.Send(new GetRefreshTokenCommand());

            return Ok(token);
        }

        [HttpGet("google")]
        public async Task<IActionResult> GetRedirectUrlAsync()
        {
            var redirectUrl = await mediator.Send(new GetRedirectUrlQuery());
            Response.Redirect(redirectUrl);

            return Ok(redirectUrl);
        }

        [HttpPost("google/sign-in")]
        public async Task<IActionResult> LoginWithGoogle([FromQuery] string code, [FromBody] string roleId)
        {
            var token = await mediator.Send(new LoginUserViaGoogleCommand(code, roleId));

            return Ok(token);
        }
    }
}
