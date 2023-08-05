using MediatR;

namespace Auth.Application.Features.User.Commands.DeleteUser
{
    public record DeleteUserCommand(string Email) : IRequest;
}
