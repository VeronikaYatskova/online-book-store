using MediatR;
using Profiles.Application.DTOs.Response;

namespace Profiles.Application.Features.Users.Queries.GetAuthors
{
    public record GetAuthorsQuery() : IRequest<IEnumerable<GetUsersResponse>>;
}
