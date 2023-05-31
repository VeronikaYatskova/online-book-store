using System.Net;
using Application.DTOs.Request;
using Application.Features.Publisher.Commands.AddPublisher;
using Application.Features.Publisher.Queries.GetAllPublishers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/publishers")]
    public class PublisherController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublisherController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPublishers()
        {
            var publishers = await mediator.Send(new GetAllPublishersQuery());

            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherById([FromRoute] string id)
        {
            var publisher = await mediator.Send(new GetPublisherByIdQuery(id));

            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> AddPublisher([FromBody] AddPublisherDto publisher)
        {
            await mediator.Send(new AddPublisherCommand(publisher));

            return Created("", publisher);
        }
    }
}
