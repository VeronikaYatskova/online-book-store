using MediatR;
using Profiles.Application.DTOs.Response;

namespace Profiles.Application.Features.Users.Queries.GetPublishers
{
    public record GetPublishersQuery() : IRequest<IEnumerable<GetUsersResponse>>;
}