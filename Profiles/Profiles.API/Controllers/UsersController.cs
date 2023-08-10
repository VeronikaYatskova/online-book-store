using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.DTOs.Request;
using Profiles.Application.Features.Users.Commands.DeleteUser;
using Profiles.Application.Features.Users.Commands.EditUser;
using Profiles.Application.Features.Users.Queries.GetAllUsers;
using Profiles.Application.Features.Users.Queries.GetAuthors;
using Profiles.Application.Features.Users.Queries.GetNormalUsers;
using Profiles.Application.Features.Users.Queries.GetPublishers;

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

        [HttpPut]
        public async Task<IActionResult> EditUserAsync([FromBody] EditUserRequest user)
        {
            await _mediator.Send(new EditUserCommand(user));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            await _mediator.Send(new DeleteUserCommand(new DeleteUserRequest
            {
                UserId = id
            }));

            return NoContent();
        }
    }
}
