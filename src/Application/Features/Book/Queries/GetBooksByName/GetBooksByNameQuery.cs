using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Book.Queries.GetBooksByName
{
    public record GetBooksByNameQuery(string name) : IRequest<IEnumerable<BookDto>>; 
}