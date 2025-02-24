using MediatR;
using SimpleBookingApp.Application.Features.Bookings.CreateBooking.Dtos.CreateBookingDTO;

namespace SimpleBookingApp.Application.Features.Bookings.CreateBooking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<CreateBookingResponseDto>
    {
        public int ResourceId { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.UtcNow;  // Başlangıç tarihi
        public DateTime DateTo { get; set; } = DateTime.UtcNow;
        public int BookedQuantity { get; set; }
    }
}
