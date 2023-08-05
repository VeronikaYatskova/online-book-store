using Auth.Application.DTOs.Request;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequest UserDataRequest, string Role) : IRequest<string>;
}
