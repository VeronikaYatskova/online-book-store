using MediatR;
using Profiles.Application.DTOs.Request;

namespace Profiles.Application.Features.Users.Commands.EditUser
{
    public record EditUserCommand(EditUserRequest UserData) : IRequest; 
}
