using Auth.Application.DTOs.Request;
using Auth.Domain.Models;
using MediatR;

namespace Auth.Application.Features.Publisher.Commands.RegisterPublisher
{
    public record RegisterPublisherCommand(
        PublisherDataRequest PublisherDataRequest, 
        AccountData AccountData) : IRequest<string>;
}
