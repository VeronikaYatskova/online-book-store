using MediatR;

namespace Auth.Application.Features.User.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Token, string Email) : IRequest<string>;
}