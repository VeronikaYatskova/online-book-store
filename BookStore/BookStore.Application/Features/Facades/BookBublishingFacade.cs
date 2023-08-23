using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Application.Features.Facades
{
    public class BookBublishingFacade : IBookPublishingFacade
    {
        private readonly IAzureService _azureService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookBublishingFacade(
            IAzureService azureService, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _azureService = azureService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task PublishBookAsync(BookPublishingMessage bookInfo)
        {
            await _azureService.CopyFileAsync(bookInfo.BookFakeName);

            var bookEntity = _mapper.Map<BookEntity>(bookInfo);

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
