using Auth.Application.DTOs.Response;
using MediatR;

namespace Auth.Application.Features.User.Queries.GetUsers
{
    public record GetUsersQuery() : IRequest<IEnumerable<GetUsersResponse>>;
}