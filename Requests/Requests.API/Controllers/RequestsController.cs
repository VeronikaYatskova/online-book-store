using Microsoft.AspNetCore.Mvc;
using Requests.BLL.DTOs.Requests;
using Requests.BLL.Services.Interfaces;

namespace Requests.API.Controllers
{
    [ApiController]
    [Route("api/requests")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsService _requestsService;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(IRequestsService requestsService, ILogger<RequestsController> logger)
        {
            _requestsService = requestsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _requestsService.GetRequestsAsync();

            return Ok(requests);
        }

        [HttpGet("{requestId}")]
        public async Task<IActionResult> GetRequestById(string requestId)
        {
            var request = await _requestsService.GetRequestByIdAsync(requestId);

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> AddRequest(AddRequestDto addRequestDto)
        {
            await _requestsService.AddRequestAsync(addRequestDto);

            return Created("", addRequestDto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(DeleteRequestDto deleteRequestDto)
        {
            await _requestsService.DeleteRequestAsync(deleteRequestDto);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRequest(UpdateRequestDto updateRequestDto)
        {
            await _requestsService.UpdateRequestAsync(updateRequestDto);

            return NoContent();
        }
    }
}
