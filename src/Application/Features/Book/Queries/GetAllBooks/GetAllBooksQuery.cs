using Application.DTOs;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Book.Queries.GetAllBooks
{
    public record GetAllBooksQuery(AwsDataWithClientUrl request) : IRequest<IEnumerable<BookDto>>;
}
