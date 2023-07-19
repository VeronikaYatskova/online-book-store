using Auth.Application.DTOs.Request;
using Auth.Domain.Models;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public record RegisterUserCommand(UserDataRequest UserDataRequest, AccountData AccountData) : IRequest<string>;
}
