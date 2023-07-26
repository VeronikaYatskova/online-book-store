using MediatR;
using Profiles.Application.DTOs.Response;

namespace Profiles.Application.Features.Users.Queries.GetNormalUsers
{
    public record GetNormalUsersQuery() : IRequest<IEnumerable<GetUsersResponse>>;
}
