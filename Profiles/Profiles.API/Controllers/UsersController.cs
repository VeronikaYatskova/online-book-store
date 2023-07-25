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

namespace Profiles.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IMediator mediator;

        public UsersController(IConfiguration config, IMediator mediator)
        {
            this.config = config;
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await mediator.Send(new GetUsersQuery());

            return Ok(users);
        }

        [HttpGet("normal-users")]
        public async Task<IActionResult> GetNormalUserAsync()
        {
            var users = await mediator.Send(new GetNormalUsersQuery());

            return Ok(users);
        }

        [HttpGet("authors")]
        public async Task<IActionResult> GetAuthorsAsync()
        {
            var authors = await mediator.Send(new GetAuthorsQuery());

            return Ok(authors);
        }

        [HttpGet("publishers")]
        public async Task<IActionResult> GetPublishersAsync()
        {
            var publishers = await mediator.Send(new GetPublishersQuery());

            return Ok(publishers);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] AddUserRequest user)
        {
            await mediator.Send(new AddUserCommand(user));

            return Created("User is created", user);
        }

        [HttpPut]
        public async Task<IActionResult> EditUserAsync([FromBody] EditUserRequest user)
        {
            await mediator.Send(new EditUserCommand(user));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string id)
        {
            await mediator.Send(new DeleteUserCommand(new DeleteUserRequest
            {
                UserId = id
            }));

            return NoContent();
        }
    }
}
