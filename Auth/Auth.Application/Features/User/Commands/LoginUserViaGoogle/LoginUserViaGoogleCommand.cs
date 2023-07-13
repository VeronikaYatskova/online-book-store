using MediatR;

namespace Auth.Application.Features.User.Commands.LoginUserViaGoogle
{
    public record LoginUserViaGoogleCommand(string Code, string RoleId) : IRequest<string>;
}