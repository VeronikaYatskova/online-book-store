using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Book.Queries.GetBookById
{
    public record GetBookByIdQuery(string id) : IRequest<BookDto>;
}