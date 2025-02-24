
using SimpleBookingApp.Application.Common.Abstracts.EmailService;

namespace SimpleBookingApp.Application.Common.Concretes.EmailManager
{
    public class EmailManager : IEmailService
    {
        public async Task SendBookingConfirmationConsoleEmailAsync(int bookingId)
        {
            Console.WriteLine($"EMAIL SENT TO admin@admin.com FOR CREATED BOOKING WITH ID {bookingId}");
            await Task.CompletedTask;
        }
    }
}
