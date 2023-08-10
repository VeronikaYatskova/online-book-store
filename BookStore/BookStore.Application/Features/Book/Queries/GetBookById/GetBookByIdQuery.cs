using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBookById
{
    public record GetBookByIdQuery(string id) : IRequest<BookDto>;
}