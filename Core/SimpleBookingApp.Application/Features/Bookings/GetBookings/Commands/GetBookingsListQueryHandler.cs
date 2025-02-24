using MediatR;
using SimpleBookingApp.Application.Features.Bookings.GetBookings.Dtos;
using SimpleBookingApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Features.Bookings.GetBookings.Commands
{
    public class GetBookingsListQueryHandler : IRequestHandler<GetBookingsListQuery, List<GetBookingsResponseDto>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingsListQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<GetBookingsResponseDto>> Handle(GetBookingsListQuery request, CancellationToken cancellationToken)
        {
            var resources = await _bookingRepository.GetAllAsync();

            return resources.Select(r => new GetBookingsResponseDto
            {
                Id = r.Id,
                ResourceId = r.ResourceId,
                BookedQuantity = r.BookedQuantity,
                DateFrom = r.DateFrom,
                DateTo = r.DateTo
            }).ToList();
        }
    }
}
