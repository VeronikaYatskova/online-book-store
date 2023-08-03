using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Request;
using BookStore.Application.Features.Book.Commands.AddBook;
using BookStore.Application.Features.Book.Commands.AddBookToFavorite;
using BookStore.Application.Features.Book.Commands.DeleteBook;
using BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite;
using BookStore.Application.Features.Book.Queries.DownloadFile;
using BookStore.Application.Features.Book.Queries.GetAllBooks;
using BookStore.Application.Features.Book.Queries.GetBookByAuthor;
using BookStore.Application.Features.Book.Queries.GetBookById;
using BookStore.Application.Features.Book.Queries.GetBooksByName;
using BookStore.Application.Features.Book.Queries.GetFavoriteBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers
{
    //[Authorize]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAwsS3Service _awsS3Service;

        public BooksController(
            IMediator mediator,
            IAwsS3Service awsS3Service)
        {
            _mediator = mediator;
            _awsS3Service = awsS3Service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _mediator.Send(new GetAllBooksQuery());

            return Ok(books);
        }

        [HttpGet("books-id/{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] string id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));

            return Ok(book);
        }

        [HttpGet("books-name/{bookName}")]
        public async Task<IActionResult> GetBookByName([FromRoute] string bookName)
        {
            var books = await _mediator.Send(new GetBooksByNameQuery(bookName));

            return Ok(books);
        }

        [HttpGet("authors/{authorId}")]
        public async Task<IActionResult> GetBookByAuthor([FromRoute] string authorId)
        {
            var books = await _mediator.Send(new GetBooksByAuthorQuery(authorId));

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync([FromForm] AddBookDto newBook)
        {
            var result = await _awsS3Service.UploadFileToBucketAsync(newBook.File);

            if (result.StatusCode != 200)
            {
                return BadRequest();
            }

            await _mediator.Send(new AddBookCommand(newBook, result.BookFakeName));

            return Created("", newBook);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] string bookId)
        {
            var request = new DeleteBookCommand(bookId);
            await _mediator.Send(request);

            return Ok();
        }

        [HttpGet("books/{documentName}")]
        public async Task<IActionResult> DownloadBook([FromRoute] string documentName)
        {
            var document = await _mediator.Send(new DownloadFileQuery(documentName));

            return File(document, "application/octet-stream", documentName);
        }

        [HttpGet("books/favorites")]
        public async Task<IActionResult> GetFavoriteBooks()
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            var favoriteBooks = await _mediator.Send(new GetFavoriteBooksQuery(currentUserGuid));

            return Ok(favoriteBooks);
        }

        [HttpPost("books/favorites/{bookId}")]
        public async Task<IActionResult> AddBookToFavourite([FromRoute] string bookId)
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            await _mediator.Send(new AddBookToFavoriteCommand(currentUserGuid, bookId));

            return Created("", bookId);
        }

        [HttpDelete("books/favorites/{bookId}")]
        public async Task<IActionResult> DeleteBookFromFavourite([FromRoute] string bookId)
        {
            var currentUserGuid = "56602e1c-541a-44ee-8280-03d4662101bb";

            await _mediator.Send(new DeleteBookFromFavoriteCommand(currentUserGuid, bookId));

            return Ok();
        }
    }
}
