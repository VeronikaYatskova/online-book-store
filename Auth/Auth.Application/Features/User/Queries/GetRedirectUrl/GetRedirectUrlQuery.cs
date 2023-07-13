using MediatR;

namespace Auth.Application.Features.User.Queries.GetRedirectUrl
{
    public record GetRedirectUrlQuery() : IRequest<string>;
}
