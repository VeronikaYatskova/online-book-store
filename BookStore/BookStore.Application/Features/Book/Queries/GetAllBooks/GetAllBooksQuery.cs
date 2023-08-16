using BookStore.Application.DTOs.General;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Features.Paging;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public record GetAllBooksQuery(BookPagesParametersDto BookPagesParametersDto) 
        : IRequest<PagedList<BookDto>>;
}
