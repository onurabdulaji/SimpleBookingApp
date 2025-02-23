using FluentValidation;
using MediatR;
using SimpleBookingApp.Application.Common.Abstracts.EmailService;
using SimpleBookingApp.Application.Features.Bookings.Commands;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Application.Validators.Booking;
using SimpleBookingApp.Domain.Entities;

namespace SimpleBookingApp.Application.Features.Bookings.Handlers
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, int>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailService _emailService;
        private readonly IValidator<CreateBookingCommand> _validator; // Validator enjekte ediliyor


        public CreateBookingHandler(IBookingRepository bookingRepository, IEmailService emailService, IValidator<CreateBookingCommand> validator)
        {
            _bookingRepository = bookingRepository;
            _emailService = emailService;
            _validator = validator;
        }

        public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {

            // FluentValidation ile doğrulama yap
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errorMessage}");
            }

            // Kaynağın uygunluğunu kontrol et
            var isAvailable = await _bookingRepository.IsResourceAvailableAsync(
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


//try
//{
//    // Kaynağın uygunluğunu kontrol et
//    var isAvailable = await _bookingRepository.IsResourceAvailableAsync(
//        request.ResourceId,
//        request.DateFrom,
//        request.DateTo,
//        request.BookedQuantity
//    );

//    if (!isAvailable)
//    {
//        throw new ValidationException("The requested resource is not available for the given period.");
//    }

//    // Kaydı oluştur
//    var booking = new Booking
//    {
//        ResourceId = request.ResourceId,
//        BookedQuantity = request.BookedQuantity,
//        DateFrom = request.DateFrom,
//        DateTo = request.DateTo
//    };

//    // Veritabanına kaydet
//    await _bookingRepository.AddAsync(booking);

//    // Başarıyla kaydedilen booking id'sini döndür
//    var bookingId = booking.Id;

//    // E-posta gönder (konsola yazdır)
//    await _emailService.SendBookingConfirmationConsoleEmailAsync(bookingId);

//    // Başarıyla kaydedilen booking id'sini döndür
//    return booking.Id;
//}
//catch (Exception ex)
//{
//    throw new InvalidOperationException("An error occurred while creating the booking.", ex);
//}