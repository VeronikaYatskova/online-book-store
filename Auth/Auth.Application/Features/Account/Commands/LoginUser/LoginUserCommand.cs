using Auth.Application.DTOs.Request;
using MediatR;

namespace Auth.Application.Features.Account.Commands.LoginUser
{
    public record LoginUserCommand(LoginUserRequest LoginUserData) : IRequest<string>;
}
