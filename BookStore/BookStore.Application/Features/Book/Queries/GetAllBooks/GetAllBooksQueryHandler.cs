using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;
using BookStore.Application.Features.Paging;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedList<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public GetAllBooksQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task<PagedList<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository.FindAllAsync() ??
                throw new NotFoundException(ExceptionMessages.BookListIsEmptyMessage);
            
            var bookPagedParameters = request.BookPagesParametersDto;

            foreach (var book in books)
            {
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
                _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            foreach (var book in booksDto)
            {
                var blob = await _azureService.GetBlobByAsync(b => b.Name == book.BookFakeName);
                    
                if (blob is not null)
                {
                    book.FileURL = blob.Uri!;
                }
            }

            return PagedList<BookDto>
                .ToPagedList(booksDto, bookPagedParameters.PageNumber, bookPagedParameters.PageSize);
        }
    }
}
