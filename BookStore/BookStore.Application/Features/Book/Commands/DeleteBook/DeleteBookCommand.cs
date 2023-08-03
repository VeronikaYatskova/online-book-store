using MediatR;

namespace BookStore.Application.Features.Book.Commands.DeleteBook
{
    public record DeleteBookCommand(string BookId) : IRequest;
}
