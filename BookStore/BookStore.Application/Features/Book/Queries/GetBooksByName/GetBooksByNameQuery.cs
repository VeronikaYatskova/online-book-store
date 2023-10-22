using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetBooksByName
{
    public record GetBooksByNameQuery(string name) : IRequest<IEnumerable<BookDto>>; 
}