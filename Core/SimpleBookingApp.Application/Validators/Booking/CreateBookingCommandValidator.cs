using FluentValidation;
using SimpleBookingApp.Application.Features.Bookings.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Validators.Booking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.ResourceId)
                .GreaterThan(0).WithMessage("Resource ID must be greater than 0.");

            RuleFor(x => x.BookedQuantity)
                .GreaterThan(0).WithMessage("Booked Quantity must be greater than 0.");

            RuleFor(x => x.DateFrom)
                .LessThan(x => x.DateTo).WithMessage("Start time must be earlier than end time.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.");

            RuleFor(x => x.DateTo)
                .GreaterThan(DateTime.UtcNow).WithMessage("End time must be in the future.");
        }
    }
}
