using Microsoft.AspNetCore.Mvc;

namespace Comments.API.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        public CommentsController()
        {
        }

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        // }

        // [HttpPost]
        // public async Task Post([FromBody] Comment comment)
        // {
        // }
    }
}
