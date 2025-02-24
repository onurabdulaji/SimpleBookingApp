using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleBookingApp.Application.Features.Bookings.CreateBooking.Commands.CreateBooking;
using SimpleBookingApp.Application.Features.Bookings.GetBookings.Commands;
using SimpleBookingApp.Domain.Entities;

namespace SimpleBookingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var result = await _mediator.Send(new GetBookingsListQuery());
            return Ok(result);
        }

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}


