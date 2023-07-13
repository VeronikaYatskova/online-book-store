using MediatR;

namespace Auth.Application.Features.User.Commands.GetRefreshToken
{
    public record GetRefreshTokenCommand() : IRequest<string>
    {
    }
}