using MediatR;

namespace Auth.Application.Features.Account.Commands.LoginUserViaGoogle
{
    public record LoginUserViaGoogleCommand(string Code, string RoleId) : IRequest<string>;
}