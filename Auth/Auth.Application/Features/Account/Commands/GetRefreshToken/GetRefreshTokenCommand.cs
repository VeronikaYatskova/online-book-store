using MediatR;

namespace Auth.Application.Features.Account.Commands.GetRefreshToken
{
    public record GetRefreshTokenCommand() : IRequest<string>;
}
