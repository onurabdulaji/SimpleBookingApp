using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Common.Abstracts.EmailService
{
    public interface IEmailService
    {
        Task SendBookingConfirmationConsoleEmailAsync(int bookingId);
    }
}
