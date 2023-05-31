using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Publisher.Queries.GetAllPublishers
{
    public record GetAllPublishersQuery() : IRequest<IEnumerable<PublisherDto>>;
}
