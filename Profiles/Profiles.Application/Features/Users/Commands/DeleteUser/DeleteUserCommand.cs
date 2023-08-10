using MediatR;
using Profiles.Application.DTOs.Request;

namespace Profiles.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(DeleteUserRequest UserData) : IRequest;
}