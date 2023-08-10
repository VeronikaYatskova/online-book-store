using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Response;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public record GetAllBooksQuery(AwsDataWithClientUrl request) : IRequest<IEnumerable<BookDto>>;
}
