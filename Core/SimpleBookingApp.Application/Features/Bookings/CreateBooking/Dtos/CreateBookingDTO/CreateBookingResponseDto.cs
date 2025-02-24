using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Features.Bookings.CreateBooking.Dtos.CreateBookingDTO
{
    public class CreateBookingResponseDto
    {
        public int BookingId { get; set; }
        public string Message { get; set; }
    }
}
