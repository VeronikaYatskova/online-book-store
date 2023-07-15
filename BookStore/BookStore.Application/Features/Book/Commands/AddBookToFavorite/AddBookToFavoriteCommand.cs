using MediatR;

namespace BookStore.Application.Features.Book.Commands.AddBookToFavorite
{
    public record AddBookToFavoriteCommand(string userId, string bookId) : IRequest;
}