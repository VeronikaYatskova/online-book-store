using Auth.Application.DTOs.Request;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequest request, string secretKey) : IRequest<string>;
}
