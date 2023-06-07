using Application.Abstractions.Contracts.Interfaces;
using Application.DTOs;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Book.Commands.AddBook;
using Application.Features.Book.Commands.AddBookToFavorite;
using Application.Features.Book.Commands.DeleteBook;
using Application.Features.Book.Commands.DeleteBookFromFavorite;
using Application.Features.Book.Commands.UploadFile;
using Application.Features.Book.Queries.DownloadFile;
using Application.Features.Book.Queries.GetAllBooks;
using Application.Features.Book.Queries.GetBookByAuthor;
using Application.Features.Book.Queries.GetBookById;
using Application.Features.Book.Queries.GetBooksByName;
using Application.Features.Book.Queries.GetFavoriteBooks;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration config;
        private readonly IMessageProducer messageProducer;

        public BooksController(IMediator mediator, IConfiguration config, IMessageProducer messageProducer)
        {
            this.mediator = mediator;
            this.config = config;
            this.messageProducer = messageProducer;
        }
        
        [HttpPost("send")]
        public IActionResult SendReply([FromBody] EmailDataRequest emailData)
        {
            var connectionData = new RabbitMqConnectionData()
            {
                HostName = config["RabbitMqConfiguration:HostName"]!,
                UserName = config["RabbitMqConfiguration:UserName"]!,
                Password = config["RabbitMqConfiguration:Password"]!,
                VirtualHost = config["RabbitMqConfiguration:VirtualHost"]!,
                ChannelName = "email-sending"
            };

            messageProducer.SendingMessage(emailData, connectionData);

            return Ok("The message is sent.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var cred = new AwsCredentials()
            {
                AwsKey = config["MinioConfiguration:MinioAccessKey"]!,
                AwsSecretKey = config["MinioConfiguration:MinioSecretKey"]!,
            };
            var bucketName = config["MinioConfiguration:BucketName"]!;
            var clientUrl = config["MinioConfiguration:ClientUrl"]!;
            var data = new AwsDataWithClientUrl()
            {
                AwsCredentials = cred,
                BucketName = bucketName,
                ClientUrl = clientUrl,
            };
            
            var books = await mediator.Send(new GetAllBooksQuery(data));

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] string id)
        {
            var book = await mediator.Send(new GetBookByIdQuery(id));

            return Ok(book);
        }

        [HttpGet("{bookName}")]
        public async Task<IActionResult> GetBookByName([FromRoute] string bookName)
        {
            var books = await mediator.Send(new GetBooksByNameQuery(bookName));

            return Ok(books);
        }

        [HttpGet("{authorName}")]
        public async Task<IActionResult> GetBookByAuthor([FromRoute] string authorName)
        {
            var books = await mediator.Send(new GetBooksByAuthorQuery(authorName));

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync([FromForm] AddBookDto newBook)
        {
            var result = await UploadFileToBucket(newBook.File);

            if (result.StatusCode != 200)
            {
                return BadRequest();
            }

            await mediator.Send(new AddBookCommand(newBook, result.BookFakeName));

            return Created("", newBook);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookAsync([FromForm] string bookId)
        {
            var cred = new AwsCredentials()
            {
                AwsKey = config["MinioConfiguration:MinioAccessKey"]!,
                AwsSecretKey = config["MinioConfiguration:MinioSecretKey"]!
            };

            var clientUrl = config["MinioConfiguration:ClientUrl"]!;
            var bucketName = config["MinioConfiguration:BucketName"]!;

            var request = new DeleteBookCommand(cred, clientUrl, bucketName, bookId);
            await mediator.Send(request);

            return Ok();
        }

        [HttpGet("books/{documentName}")]
        public async Task<IActionResult> DownloadBook(string documentName)
        {
            var cred = new AwsCredentials()
            {
                AwsKey = config["MinioConfiguration:MinioAccessKey"]!,
                AwsSecretKey = config["MinioConfiguration:MinioSecretKey"]!
            };

            var requestData = new Application.DTOs.AwsDataWithClientUrl()
            {
                AwsCredentials = cred,
                FileName = documentName,
                BucketName = config["MinioConfiguration:BucketName"]!,
                ClientUrl = config["MinioConfiguration:ClientUrl"]!
            };

            var document = await mediator.Send(new DownloadFileQuery(requestData));

            return File(document, "application/octet-stream", documentName);
        }

        [HttpGet("books/favorites")]
        public async Task<IActionResult> GetFavoriteBooks()
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            var favoriteBooks = await mediator.Send(new GetFavoriteBooksQuery(currentUserGuid));

            return Ok(favoriteBooks);
        }

        [HttpPost("books/favorites/{bookId}")]
        public async Task<IActionResult> AddBookToFavourite([FromRoute] string bookId)
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            await mediator.Send(new AddBookToFavoriteCommand(currentUserGuid, bookId));

            return Created("", bookId);
        }

        [HttpDelete("books/favorites/{bookId}")]
        public async Task<IActionResult> DeleteBookFromFavourite([FromRoute] string bookId)
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            await mediator.Send(new DeleteBookFromFavoriteCommand(currentUserGuid, bookId));

            return Ok();
        }

        private async Task<S3ResponseDto> UploadFileToBucket(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExtension = Path.GetExtension(file.FileName);
            var objName = $"{Guid.NewGuid()}-{DateTime.Now.ToString("yyyy'-'MM'-'dd")}{fileExtension}";

            var s3obj = new Domain.Entities.S3Object()
            {
                BucketName = config["MinioConfiguration:BucketName"]!,
                InputStream = memoryStream,
                Name = objName
            };

            var cred = new AwsCredentials()
            {
                AwsKey = config["MinioConfiguration:MinioAccessKey"]!,
                AwsSecretKey = config["MinioConfiguration:MinioSecretKey"]!
            };

            var clientUrl = config["MinioConfiguration:ClientUrl"];
            var result = await mediator.Send(new UploadFileCommand(s3obj, cred, clientUrl!));

            return result;
        }
    }
}
