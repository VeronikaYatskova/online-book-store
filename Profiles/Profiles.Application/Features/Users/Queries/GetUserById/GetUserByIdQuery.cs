using MediatR;
using Profiles.Application.DTOs.Response;

namespace Profiles.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string UserId) : IRequest<GetUsersResponse>;
}
