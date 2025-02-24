using FluentValidation;
using MediatR;
using SimpleBookingApp.Application.Common.Abstracts.EmailService;
using SimpleBookingApp.Application.Features.Bookings.CreateBooking.Dtos.CreateBookingDTO;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Domain.Entities;

namespace SimpleBookingApp.Application.Features.Bookings.CreateBooking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingResponseDto>
    {
        private readonly IValidator<CreateBookingCommand> _validator;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailService _emailService;

        // Constructor
        public CreateBookingCommandHandler(
            IValidator<CreateBookingCommand> validator,
            IBookingRepository bookingRepository,
            IEmailService emailService)
        {
            _validator = validator;
            _bookingRepository = bookingRepository;
            _emailService = emailService;

        }

        public async Task<CreateBookingResponseDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // FluentValidation ile doğrulama yap
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errorMessage}");
            }

            // Kaynağın uygunluğunu kontrol et
            var isAvailable = await _bookingRepository.IsBookingConflictAsync(
                request.ResourceId,
                request.DateFrom,
                request.DateTo,
                request.BookedQuantity
            );


            if (!isAvailable)
            {
                throw new ValidationException("The requested resource is not available for the given period.");
            }

            // Kaydı oluştur
            var booking = new Booking
            {
                ResourceId = request.ResourceId,
                BookedQuantity = request.BookedQuantity,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            };

            // Veritabanına kaydet
            await _bookingRepository.AddAsync(booking);

            var bookingId = booking.Id;

            // E-posta gönder (konsola yazdır)
            await _emailService.SendBookingConfirmationConsoleEmailAsync(bookingId);

            return booking.Id;
        }
    }
}
