using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetFavoriteBooks
{
    public record GetFavoriteBooksQuery(string userId) : IRequest<IEnumerable<BookDto>>;
}