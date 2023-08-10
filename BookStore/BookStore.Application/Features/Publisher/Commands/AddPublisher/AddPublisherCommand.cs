using BookStore.Application.DTOs.Request;
using MediatR;

namespace BookStore.Application.Features.Publisher.Commands.AddPublisher
{
    public record AddPublisherCommand(AddPublisherDto publisher) : IRequest;
}