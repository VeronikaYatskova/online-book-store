using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Book.Queries.GetFavoriteBooks
{
    public record GetFavoriteBooksQuery(string userId) : IRequest<IEnumerable<BookDto>>;
}