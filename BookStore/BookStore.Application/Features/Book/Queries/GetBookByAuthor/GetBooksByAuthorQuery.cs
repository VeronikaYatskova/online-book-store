using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBookByAuthor
{
    public record GetBooksByAuthorQuery(string authorName) : IRequest<IEnumerable<BookDto>>;
}