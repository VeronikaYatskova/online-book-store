using Auth.Application.DTOs.Request;
using Auth.Domain.Models;
using MediatR;

namespace Auth.Application.Features.Author.Commands.RegisterAuthor
{
    public record RegisterAuthorCommand(AuthorDataRequest AuthorDataRequest, AccountData AccountData) : IRequest<string>;
}
