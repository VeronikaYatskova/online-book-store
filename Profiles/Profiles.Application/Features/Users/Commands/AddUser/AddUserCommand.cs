using MediatR;
using Profiles.Application.DTOs.Request;

namespace Profiles.Application.Features.Users.Commands.AddUser
{
    public record AddUserCommand(AddUserRequest UserData) : IRequest;
}