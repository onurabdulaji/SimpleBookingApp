using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBookingApp.Application.Features.Bookings.CreateBooking.Commands.CreateBooking;
using System.Reflection;

namespace SimpleBookingApp.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(CreateBookingCommandValidator).Assembly);
            // SmtpSettings ve EmailManager
            return services;
        }
    }
}
