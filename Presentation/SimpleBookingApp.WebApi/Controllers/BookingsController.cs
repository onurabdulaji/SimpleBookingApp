using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SimpleBookingApp.Application.Features.Bookings.Commands;

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

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { BookingId = result, Message = "Booking successfully created." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = "Validation failed", Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request", Error = ex.Message });
            }
        }
    }
}



//RESTful API
//return CreatedAtAction(nameof(GetBooking), new { id = result }, new { BookingId = result, Message = "Booking successfully created." });
