using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Features.Book.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private readonly BlobStorageSettings _blobStorageSettings;

        public GetAllBooksQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureService azureService,
            IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureService = azureService;
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.BooksRepository.FindAllAsync() ??
                throw new NotFoundException(ExceptionMessages.BookListIsEmptyMessage);

            foreach (var book in books)
            {
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
                _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
                _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
            }

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            await _azureService.LoadRelatedData(booksDto);

            return booksDto;
        }
    }
}
