using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public record GetAllBooksQuery() : IRequest<IEnumerable<BookDto>>;
}
