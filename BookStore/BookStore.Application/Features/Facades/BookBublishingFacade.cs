using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using BookStore.Domain.Entities;
using Microsoft.Extensions.Options;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Features.Facades
{
    public class BookBublishingFacade : IBookPublishingFacade
    {
        private readonly IAzureService _azureService;
        private readonly BlobStorageSettings _blobStorageSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookBublishingFacade(
            IAzureService azureService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _azureService = azureService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task PublishBookAsync(BookPublishingMessage bookInfo)
        {
            await _azureService.CopyFileAsync(
                bookInfo.BookFakeName, 
                _blobStorageSettings.RequestedBooksContainerName, 
                _blobStorageSettings.PublishedBooksContainerName);

            var bookEntity = _mapper.Map<BookEntity>(bookInfo);
            bookEntity.BookCoverFakeName = bookInfo.BookPictureURL;

            await _unitOfWork.BooksRepository.CreateAsync(bookEntity);

            var book = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == bookEntity.BookGuid);
            
            if (book is null)
            {
                await AddBookToAuthorAsync(bookInfo, bookEntity);
            }           

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task AddBookToAuthorAsync(BookPublishingMessage bookInfo, BookEntity bookEntity)
        {
            if (bookInfo.AuthorsGuid.Count() > 1)
            {
                foreach (var authorId in bookInfo.AuthorsGuid)
                {
                    var bookAuthorEntity = new BookAuthorEntity
                    {
                        Guid = Guid.NewGuid(),
                        BookGuid = bookEntity.BookGuid,
                        AuthorGuid = Guid.Parse(authorId),
                    };

                    await _unitOfWork.AuthorsBooksRepository.CreateAsync(bookAuthorEntity);
                }
            }
            else
            {
                var bookAuthorEntity = new BookAuthorEntity
                {
                    Guid = Guid.NewGuid(),
                    BookGuid = bookEntity.BookGuid,
                    AuthorGuid = Guid.Parse(bookInfo.AuthorsGuid.First()),
                };

                await _unitOfWork.AuthorsBooksRepository.CreateAsync(bookAuthorEntity);
            }
        } 
    }
}
