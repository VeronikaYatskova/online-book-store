using System.Net;
using AuthProfilesServices.Communication.Additional;
using AutoMapper;
using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;
using RequestsBookStore.Communication.Models;

namespace BookStore.Infrastructure.Consumers
{
    public class BookPublishingConsumer : IConsumer<BookPublishingMessage>
    {
        private readonly IAzureService _azureService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookPublishingConsumer> _logger;

        public BookPublishingConsumer(
            IAzureService azureService, 
            ILogger<BookPublishingConsumer> logger, 
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _azureService = azureService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<BookPublishingMessage> context)
        {
            _logger.LogInformation("Book publishing message is recevied.");

            var bookInfo = context.Message;

            await _azureService.CopyFileAsync(context.Message.BookFakeName);

            var bookEntity = _mapper.Map<BookEntity>(bookInfo);

            await _unitOfWork.BooksRepository.CreateAsync(bookEntity);

            var book = await _unitOfWork.BooksRepository
                .FindByConditionAsync(b => b.BookGuid == bookEntity.BookGuid);
            
            if (book is null)
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

            await _unitOfWork.SaveChangesAsync();

            await context.RespondAsync(new BookPublishedEvent { StatusCode = 200 });
        }
    }
}
