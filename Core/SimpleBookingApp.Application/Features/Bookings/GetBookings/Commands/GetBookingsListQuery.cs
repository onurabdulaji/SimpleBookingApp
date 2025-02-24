using MediatR;
using SimpleBookingApp.Application.Features.Bookings.GetBookings.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Features.Bookings.GetBookings.Commands
{
    public class GetBookingsListQuery : IRequest<List<GetBookingsResponseDto>>
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.UtcNow;  // Başlangıç tarihi
        public DateTime DateTo { get; set; } = DateTime.UtcNow.AddDays(1);  // Bitiş tarihi (örneğin 1 gün sonrası)
        public int BookedQuantity { get; set; }
    }
}
