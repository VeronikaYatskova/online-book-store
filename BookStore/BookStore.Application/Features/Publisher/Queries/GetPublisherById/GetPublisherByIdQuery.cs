using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Publisher.Queries.GetAllPublishers
{
    public record GetPublisherByIdQuery(string id) : IRequest<PublisherDto>;
}