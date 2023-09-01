using Auth.Application.DTOs.Request;
using MassTransit;
using MediatR;

namespace Auth.Application.Features.User.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserRequest UserDataRequest, string Role, IPublishEndpoint _publishEndpoint) : IRequest;
}
