using MediatR;

namespace BookStore.Application.Features.Book.Commands.AddBookToFavorite
{
    public record AddBookToFavoriteCommand(string UserId, string BookId) : IRequest;
}