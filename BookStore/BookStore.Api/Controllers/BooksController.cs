using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Application.DTOs.Request;
using BookStore.Application.Features.Book.Commands.AddBookToFavorite;
using BookStore.Application.Features.Book.Commands.DeleteBook;
using BookStore.Application.Features.Book.Commands.DeleteBookFromFavorite;
using BookStore.Application.Features.Book.Queries.DownloadFile;
using BookStore.Application.Features.Book.Queries.GetAllBooks;
using BookStore.Application.Features.Book.Queries.GetBookByAuthor;
using BookStore.Application.Features.Book.Queries.GetBookById;
using BookStore.Application.Features.Book.Queries.GetBooksByName;
using BookStore.Application.Features.Book.Queries.GetFavoriteBooks;
using BookStore.Application.Features.Comment.Commands;
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
        private readonly IAzureService _azureService;

        public BooksController(
            IMediator mediator,
            IAzureService azureService)
        {
            _mediator = mediator;
            _azureService = azureService;
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

        [HttpGet("azurite")]
        public async Task<IActionResult> GetBookFromCloud()
        {
            return Ok(await _azureService.GetAllAsync());
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

        [HttpPost("comments")]
        public async Task<IActionResult> AddBookComment([FromBody] BookCommentDto bookCommentDto)
        {
            await _mediator.Send(new AddBookCommentCommand(bookCommentDto));

            return Created("", bookCommentDto);
        }
    }
}
