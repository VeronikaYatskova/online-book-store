
using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBooksByCategory
{
    public record GetBooksByCategoryQuery(string CategoryId) : IRequest<IEnumerable<BookDto>>;
}