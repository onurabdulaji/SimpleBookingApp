using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBookingApp.Application.Features.Resources.ListResources.Commands.GetResourcesList;

namespace SimpleBookingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetResources()
        {
            var result = await _mediator.Send(new GetResourceListQuery());
            return Ok(result);
        }
    }
}
