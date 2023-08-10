using Microsoft.AspNetCore.Mvc;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.Services.Interfaces;

namespace Requests.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsService _requestsService;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(IRequestsService requestsService, ILogger<RequestsController> logger)
        {
            _requestsService = requestsService;
            _logger = logger;
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _requestsService.GetRequestsAsync();

            return Ok(requests);
        }

        [HttpGet("requests/{requestId}")]
        public async Task<IActionResult> GetRequestById(string requestId)
        {
            var request = await _requestsService.GetRequestByIdAsync(requestId);

            return Ok(request);
        }

        [HttpGet("publishers/{publisherId}/requests")]
        public async Task<IActionResult> GetPublishersRequest(string publisherId)
        {
            var requests = await _requestsService.GetPublishersRequests(publisherId);

            return Ok(requests);
        }

        [HttpGet("users/{userId}/requests")]
        public async Task<IActionResult> GetUsersRequests(string userId)
        {
            var requests = await _requestsService.GetUsersRequests(userId);

            return Ok(requests);
        }

        [HttpPost("requests")]
        public async Task<IActionResult> AddRequest([FromForm] AddRequestDto addRequestDto)
        {
            await _requestsService.AddRequestAsync(addRequestDto);

            return Created("", addRequestDto);
        }

        [HttpDelete("requests")]
        public async Task<IActionResult> DeleteRequest([FromBody] DeleteRequestDto deleteRequestDto)
        {
            await _requestsService.DeleteRequestAsync(deleteRequestDto);

            return NoContent();
        }

        [HttpPut("requests")]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestDto updateRequestDto)
        {
            await _requestsService.UpdateRequestAsync(updateRequestDto);

            return NoContent();
        }

        [HttpPost("requests/{requestId}")]
        public async Task<IActionResult> PublishBook([FromRoute] string requestId, [FromBody] AddBookDto addBookDto)
        {
            await _requestsService.PublishBookAsync(requestId, addBookDto);

            return Created("", addBookDto);
        }
    }
}
