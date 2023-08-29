using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Response;
using AutoMapper;
using MediatR;
using OnlineBookStore.Exceptions.Exceptions;
using BookStore.Domain.Exceptions;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using Microsoft.Extensions.Options;

namespace BookStore.Application.Features.Book.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly IAzureService _azureService;
        private readonly BlobStorageSettings _blobStorageSettings;

        public GetBookByIdQueryHandler(
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

        public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == Guid.Parse(request.id)) ??
                    throw new NotFoundException(ExceptionMessages.BookNotFoundMessage);
            
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Category);
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, book => book.Publisher);
            _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, book => book.BookAuthors);
        
            var bookDto = _mapper.Map<BookDto>(book);
            
            var blobUri = await _azureService.GetBlobByNameAsync(
                book.BookFakeName!, 
                _blobStorageSettings.PublishedBooksContainerName);
                    
            if (blobUri is not null)
            {
                bookDto.FileURL = blobUri!;
            }

            var blobCoverUri = await _azureService.GetBlobByNameAsync(
                book.BookFakeName!, 
                _blobStorageSettings.BookCoversContainerName);

            if (blobCoverUri is not null)
            {
                bookDto.BookCoverFakeName = blobCoverUri!;
            }

            return bookDto;
        }
    }
}
