using FluentValidation;
using Moq;
using SimpleBookingApp.Application.Common.Abstracts.EmailService;
using SimpleBookingApp.Application.Features.Bookings.Commands;
using SimpleBookingApp.Application.Features.Bookings.Handlers;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Domain.Entities;

namespace SimpleBookingApp.UnitTest.Handlers
{
    public class CreateBookingHandlerTests
    {
        private readonly Mock<IBookingRepository> _mockBookingRepository;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IValidator<CreateBookingCommand>> _mockValidator;
        private readonly CreateBookingHandler _handler;

        public CreateBookingHandlerTests()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockValidator = new Mock<IValidator<CreateBookingCommand>>();

            _handler = new CreateBookingHandler(
            _mockBookingRepository.Object,  // 1. Repository
            _mockEmailService.Object,       // 2. EmailService
            _mockValidator.Object           // 3. Validator (Doğru sırada!)
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateBooking_WhenBookingIsValid()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                ResourceId = 1,
                BookedQuantity = 5,
                DateFrom = DateTime.Now.AddHours(1),
                DateTo = DateTime.Now.AddHours(2)
            };

            // Validator başarılı olacak şekilde ayarlandı
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Kaynağın uygun olduğunu varsayıyoruz
            _mockBookingRepository.Setup(r => r.IsResourceAvailableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Booking nesnesinin ID'sini set etmek için Callback ekliyoruz
            _mockBookingRepository.Setup(r => r.AddAsync(It.IsAny<Booking>()))
                .Callback<Booking>(b => b.Id = 1) // ID'yi set ediyoruz
                .Returns(Task.CompletedTask);

            // E-posta servisi simüle ediliyor
            _mockEmailService.Setup(e => e.SendBookingConfirmationConsoleEmailAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(0, result);  // Booking ID sıfırdan farklı olmalı (başarıyla oluşturuldu)
            _mockBookingRepository.Verify(r => r.AddAsync(It.IsAny<Booking>()), Times.Once);  // AddAsync yalnızca bir kez çağrılmalı
            _mockEmailService.Verify(e => e.SendBookingConfirmationConsoleEmailAsync(It.IsAny<int>()), Times.Once);  // E-posta gönderimi yapılmalı
        }
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenBookingIsInvalid()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                ResourceId = 0, // Geçersiz ResourceId
                BookedQuantity = 5,
                DateFrom = DateTime.Now.AddHours(1),
                DateTo = DateTime.Now.AddHours(2)
            };

            // Validator başarısız olacak şekilde ayarlandı
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult
            {
                Errors = { new FluentValidation.Results.ValidationFailure("ResourceId", "Resource ID must be greater than 0.") }
            });

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenResourceIsNotAvailable()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                ResourceId = 1,
                BookedQuantity = 10,
                DateFrom = DateTime.Now.AddHours(1),
                DateTo = DateTime.Now.AddHours(2)
            };

            // Validator başarılı olacak şekilde ayarlandı
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Kaynağın uygun olmadığı varsayıldı
            _mockBookingRepository.Setup(r => r.IsResourceAvailableAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

