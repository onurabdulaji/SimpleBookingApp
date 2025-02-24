using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Features.Bookings.CreateBooking.Dtos.CreateBookingDTO
{
    public class CreateBookingRequestDto
    {
        public int ResourceId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int BookedQuantity { get; set; }
    }
}
