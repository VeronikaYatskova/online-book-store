using Comments.API.Filters;
using Comments.BLL.DTOs.Request;
using Comments.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Comments.API.Controllers
{
    [ValidationFilter]
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookCommentsAsync([FromRoute] string bookId)
        {
            var comments = await _commentsService.GetCommentsByBookIdAsync(bookId);

            return Ok(comments);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentRequest updateCommentRequest)
        {
            await _commentsService.UpdateCommentAsync(updateCommentRequest);

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync([FromRoute] string commentId)
        {
            await _commentsService.DeleteCommentAsync(commentId);

            return NoContent();
        }
    }
}
