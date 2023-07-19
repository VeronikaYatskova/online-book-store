using Auth.Application.DTOs.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.Features.User.Queries.GetUsers;
using Auth.Application.Features.User.Commands.LoginUser;
using Auth.Application.Features.User.Queries.GetRedirectUrl;
using Auth.Application.Features.User.Commands.LoginUserViaGoogle;
using Auth.Domain.Models;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Application.Features.Account.Commands.RegisterAccount;
using Auth.Application.Features.Publisher.Commands.RegisterPublisher;
using Auth.Application.Features.Author.Commands.RegisterAuthor;
using Auth.Application.Features.Account.Commands.GetRefreshToken;

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

        [HttpPost("users/sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            var accountData = await mediator.Send(new RegisterAccountCommand(request.RegisterAccountData, UserRolesConstants.UserRole));
            
            var token = await mediator.Send(new RegisterUserCommand(request.UserDataRequest, accountData));

            return Created("User was created.", token);
        }

        [HttpPost("publishers/sign-up")]
        public async Task<IActionResult> RegisterPublisher([FromBody] RegisterPublisherRequest request)
        {
            var accountData = await mediator.Send(new RegisterAccountCommand(request.RegisterAccountData, UserRolesConstants.PublisherRole));
            
            var token = await mediator.Send(new RegisterPublisherCommand(request.PublisherDataRequest, accountData));

            return Created("User was created.", token);
        }

        [HttpPost("authors/sign-up")]
        public async Task<IActionResult> RegisterAuthor([FromBody] RegisterAuthorRequest request)
        {
            var accountData = await mediator.Send(new RegisterAccountCommand(request.RegisterAccountData, UserRolesConstants.AuthorRole));
            
            var token = await mediator.Send(new RegisterAuthorCommand(request.AuthorDataRequest, accountData));

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
