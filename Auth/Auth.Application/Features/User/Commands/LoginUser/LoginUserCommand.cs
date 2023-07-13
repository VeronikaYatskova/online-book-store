using Auth.Application.DTOs.Request;
using MediatR;

namespace Auth.Application.Features.User.Commands.LoginUser
{
    public record LoginUserCommand(LoginUserRequest request) : IRequest<string>;
}