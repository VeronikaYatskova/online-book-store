using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.AddUser;
using Profiles.Application.Features.Users.Commands.DeleteUser;
using Profiles.Application.Features.Users.Commands.EditUser;
using Profiles.Application.Features.Users.Queries.GetAllUsers;
using Profiles.Application.Features.Users.Queries.GetAuthors;
using Profiles.Application.Features.Users.Queries.GetNormalUsers;
using Profiles.Application.Features.Users.Queries.GetPublishers;
using Profiles.Application.Features.Users.Queries.GetUserById;

namespace Profiles.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public UsersController(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _mediator.Send(new GetUsersQuery());

            return Ok(users);
        }

        [HttpGet("normal-users")]
        public async Task<IActionResult> GetNormalUserAsync()
        {
            var users = await _mediator.Send(new GetNormalUsersQuery());

            return Ok(users);
        }

        [HttpGet("authors")]
        public async Task<IActionResult> GetAuthorsAsync()
        {
            var authors = await _mediator.Send(new GetAuthorsQuery());

            return Ok(authors);
        }

        [HttpGet("publishers")]
        public async Task<IActionResult> GetPublishersAsync()
        {
            var publishers = await _mediator.Send(new GetPublishersQuery());

            return Ok(publishers);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(userId));

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest user)
        {
            await _mediator.Send(new AddUserCommand(user));

            return Created("User is created", user);
        }

        [HttpPut]
        public async Task<IActionResult> EditUserAsync([FromBody] EditUserRequest user)
        {
            await _mediator.Send(new EditUserCommand(user));

            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
        {
            await _mediator.Send(new DeleteUserCommand(new DeleteUserRequest
            {
                UserId = id
            }));

            return NoContent();
        }
    }
}
