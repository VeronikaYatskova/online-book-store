using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using BookStore.Application.Exceptions;

namespace BookStore.Application.Features.Book.Queries.GetBooksByName
{
    public class GetBooksByNameQueryHandler : IRequestHandler<GetBooksByNameQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;

        public GetBooksByNameQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureService azureService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetBooksByNameQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository
                .FindAllAsync(b => b.BookName == request.name) ??
                    throw new NotFoundException(ExceptionMessages.BooksNotFoundMessage);
            
            foreach (var book in books)
            {
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
                _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            foreach (var book in booksDto)
            {
                var blob = await _azureService.GetBlobByAsync(b => b.Name == book.BookFakeName)
                    ?? throw new FileDoesNotExistException();
                    
                book.FileURL = blob.Uri!;
            }

            return booksDto;
        }
    }
}
