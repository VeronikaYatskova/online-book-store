using BookStore.Application.Abstractions.Contracts.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Messages.Models.Messages;

namespace BookStore.Infrastructure.Consumers
{
    public class BookPublishingConsumer : IConsumer<BookPublishingMessage>
    {
        private readonly IBookPublishingFacade _bookPublishingFacade;
        private readonly ILogger<BookPublishingConsumer> _logger;

        public BookPublishingConsumer(
            IBookPublishingFacade bookPublishingFacade,
            ILogger<BookPublishingConsumer> logger)
        {
            _logger = logger;
            _bookPublishingFacade = bookPublishingFacade;
        }

        public async Task Consume(ConsumeContext<BookPublishingMessage> context)
        {
            _logger.LogInformation("Book publishing message is recevied.");

            var bookInfo = context.Message;

            await _bookPublishingFacade.PublishBookAsync(bookInfo);
            
            await context.RespondAsync(new BookPublishedMessage { StatusCode = 200 });
        }
    }
}
