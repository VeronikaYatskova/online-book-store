using Application.DTOs.Request;
using MediatR;

namespace Application.Features.Publisher.Commands.AddPublisher
{
    public record AddPublisherCommand(AddPublisherDto publisher) : IRequest;
}