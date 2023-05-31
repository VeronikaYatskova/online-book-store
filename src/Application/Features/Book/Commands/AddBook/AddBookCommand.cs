using Application.DTOs.Request;
using MediatR;

namespace Application.Features.Book.Commands.AddBook
{
    public record AddBookCommand(AddBookDto book, string bookFakeName) : IRequest;
}