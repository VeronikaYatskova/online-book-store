using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public record GetBooksByAuthorQuery(string AuthorId) : IRequest<IEnumerable<BookDto>>;
}
