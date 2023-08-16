using Auth.Application.DTOs.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.Features.User.Queries.GetUsers;
using Auth.Application.Features.User.Commands.LoginUser;
using Auth.Domain.Models;
using Auth.Application.Features.User.Commands.RegisterUser;
using Auth.Application.Features.User.Commands.GetRefreshToken;
using Auth.Application.Features.User.Commands.DeleteUser;
using Auth.Application.Abstractions.Interfaces.Services;
using Auth.Application.Features.User.Commands.ConfirmEmail;
using MassTransit;
using OnlineBookStore.Messages.Models.Messages;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuthController(
            IMediator mediator, 
            ITokenService tokenService, 
            IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _tokenService = tokenService;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new GetUsersQuery());

            return Ok(users);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var token = await _mediator.Send(new LoginUserCommand(request));

            return Ok(token);
        }

        [HttpPost("users/sign-up")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            await _mediator.Send(new RegisterUserCommand(request, UserRolesConstants.UserRole));

            await ConfirmEmail(request.Email);

            return Created("User was created.", request);
        }

        [HttpPost("publishers/sign-up")]
        public async Task<IActionResult> RegisterPublisher([FromBody] RegisterUserRequest request)
        {
            await _mediator.Send(new RegisterUserCommand(request, UserRolesConstants.PublisherRole));
            
            await ConfirmEmail(request.Email);

            return Created("User was created.", request);
        }

        [HttpPost("authors/sign-up")]
        public async Task<IActionResult> RegisterAuthor([FromBody] RegisterUserRequest request)
        {
            await _mediator.Send(new RegisterUserCommand(request, UserRolesConstants.AuthorRole));

            await ConfirmEmail(request.Email);

            return Created("User was created.", request);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string verificationToken, string email)
        {
            var result = await _mediator.Send(new ConfirmEmailCommand(verificationToken, email));

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserRequest request)
        {
            await _mediator.Send(new DeleteUserCommand(request.Email));

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = await _mediator.Send(new GetRefreshTokenCommand());

            return Ok(token);
        }

        private async Task ConfirmEmail(string email)
        {
            var verificationToken = _tokenService.CreateVerificationToken();
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", 
                new { verificationToken, email = email }, Request.Scheme);

            await _publishEndpoint.Publish(new EmailConfirmationMessage { ConfirmationLink = confirmationLink!,
                To = email });
        }
    }
}
