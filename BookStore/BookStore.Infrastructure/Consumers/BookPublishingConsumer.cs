using AutoMapper;
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
        private readonly IAzureService _azureService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookPublishingConsumer> _logger;
        private readonly ITemplateGenerator _templateGenerator;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly BlobStorageSettings _blobStorageSettings;

        public BookPublishingConsumer(
            IAzureService azureService,
            ILogger<BookPublishingConsumer> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ITemplateGenerator templateGenerator,
            IPdfGenerator pdfGenerator,
            IOptions<BlobStorageSettings> blobStorageSettings)
        {
            _azureService = azureService;
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _templateGenerator = templateGenerator;
            _pdfGenerator = pdfGenerator;
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task Consume(ConsumeContext<BookPublishingMessage> context)
        {
            _logger.LogInformation("Book publishing message is recevied.");

            var bookInfo = context.Message;

            // await _azureService.CopyFileAsync(context.Message.BookFakeName);

            var bookEntity = _mapper.Map<BookEntity>(bookInfo);
            await _unitOfWork.BooksRepository.CreateAsync(bookEntity);
            await _unitOfWork.SaveChangesAsync();            

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
            
            var pdfFile = GeneratePdfFile(book!);
            using var memoryStream = new MemoryStream(pdfFile);

            await _azureService.UploadAsync(
                new FormFile(memoryStream, 0, memoryStream.Length, "pdf file", "pdf"),
                _blobStorageSettings.RequestReportsContainerName);
            
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
