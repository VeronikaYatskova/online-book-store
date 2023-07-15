using System.Net;
using BookStore.Application.DTOs.Request;
using BookStore.Application.Features.Publisher.Commands.AddPublisher;
using BookStore.Application.Features.Publisher.Queries.GetAllPublishers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers
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
