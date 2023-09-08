using Microsoft.AspNetCore.Authorization;
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
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(
            IRequestsService requestsService,
            ILogger<RequestsController> logger,
            IBlobStorageService blobStorageService)
        {
            _requestsService = requestsService;
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        [Authorize(Roles = "Publisher")]
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _requestsService.GetRequestsAsync();

            return Ok(requests);
        }

        [Authorize(Roles = "Publisher")]
        [HttpGet("requests/{requestId}")]
        public async Task<IActionResult> GetRequestById(string requestId)
        {
            var request = await _requestsService.GetRequestByIdAsync(requestId);

            return Ok(request);
        }

        [Authorize(Roles = "Publisher")]
        [HttpGet("publishers/{publisherId}/requests")]
        public async Task<IActionResult> GetPublishersRequest(string publisherId)
        {
            var requests = await _requestsService.GetPublishersRequests(publisherId);

            return Ok(requests);
        }

        [Authorize(Roles = "Author")]
        [HttpGet("users/{userId}/requests")]
        public async Task<IActionResult> GetUsersRequests(string userId)
        {
            var requests = await _requestsService.GetUsersRequests(userId);

            return Ok(requests);
        }

        [Authorize(Roles = "Author")]
        [HttpPost("requests")]
        public async Task<IActionResult> AddRequest([FromForm] AddRequestDto addRequestDto)
        {
            await _requestsService.AddRequestAsync(addRequestDto);

            return Created("", addRequestDto);
        }

        [Authorize(Roles = "Publisher")]
        [HttpDelete("requests")]
        public async Task<IActionResult> DeleteRequest([FromBody] DeleteRequestDto deleteRequestDto)
        {
            await _requestsService.DeleteRequestAsync(deleteRequestDto);

            return NoContent();
        }

        [Authorize(Roles = "Publisher")]
        [HttpPut("requests")]
        public async Task<IActionResult> UpdateRequest([FromBody] UpdateRequestDto updateRequestDto)
        {
            await _requestsService.UpdateRequestAsync(updateRequestDto);

            return NoContent();
        }

        [Authorize(Roles = "Publisher")]
        [HttpPost("requests/{requestId}")]
        public async Task<IActionResult> PublishBook([FromRoute] string requestId, [FromBody] AddBookDto addBookDto)
        {
            await _requestsService.PublishBookAsync(requestId, addBookDto);

            return Created("", addBookDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetFromCloud()
        {
            return Ok(await _blobStorageService.GetAllAsync());
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetBlobFromCloud([FromRoute] string fileName)
        {
            return Ok(await _blobStorageService.GetBlobByNameAsync(fileName));
        }
    }
}
