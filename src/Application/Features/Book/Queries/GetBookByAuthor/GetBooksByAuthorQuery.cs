using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Book.Queries.GetBookByAuthor
{
    public record GetBooksByAuthorQuery(string authorName) : IRequest<IEnumerable<BookDto>>;
}