using BookStore.Application.DTOs.Request;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.AddBook
{
    public record AddBookCommand(AddBookDto book, string bookFakeName) : IRequest;
}