using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.Services.CloudServices.Azurite.Models;
using BookStore.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineBookStore.Messages.Models.Messages;
using PdfGenerator.Interfaces;
using PdfGenerator.Models;

namespace BookStore.Infrastructure.Consumers
{
    public class BookPublishingConsumer : IConsumer<BookPublishingMessage>
    {
        private readonly IBookPublishingFacade _bookPublishingFacade;
        private readonly ILogger<BookPublishingConsumer> _logger;
        private readonly ITemplateGenerator _templateGenerator;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly BlobStorageSettings _blobStorageSettings;

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

        private byte[] GeneratePdfFile(BookEntity book)
        {
            var pdfGenerator = _templateGenerator.BookPublishedMessageTemplateGenerator();

            var bookInPdf = _mapper.Map<BookInPdf>(book);
            
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, b => b.Category);
            _unitOfWork.BooksRepository.LoadRelatedDataWithReference(book, b => b.Publisher);
            _unitOfWork.BooksRepository.LoadRelatedDataWithCollection(book, b => b.BookAuthors);

            bookInPdf.Publisher = book.Publisher.Email;
            bookInPdf.Category = book.Category.CategoryName; 
            
            var htmlTemplate = pdfGenerator.GenerateHtmlTemplate(bookInPdf);
            var pdfFile = _pdfGenerator.CreatePDF(htmlTemplate);

            return pdfFile;
        }
    }
}
