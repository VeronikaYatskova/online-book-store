using MediatR;
using Profiles.Application.DTOs.Response;

namespace Profiles.Application.Features.Users.Queries.GetAllUsers
{
    public record GetUsersQuery() : IRequest<IEnumerable<GetUsersResponse>>;
}
