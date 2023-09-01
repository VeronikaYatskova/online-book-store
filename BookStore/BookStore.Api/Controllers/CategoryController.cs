using System.Threading.Tasks;
using BookStore.Application.Features.Category.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BookStore.WebApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            Log.Logger.Information("You are about to see all categories.");

            var categories = await _mediator.Send(new GetAllCategoriesQuery());

            return Ok(categories);
        }
    }
}
