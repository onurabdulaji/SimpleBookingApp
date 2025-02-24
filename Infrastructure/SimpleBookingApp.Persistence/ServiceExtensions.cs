using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleBookingApp.Application.Common.Abstracts.EmailService;
using SimpleBookingApp.Application.Common.Concretes.EmailManager;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Persistence.Behaviors;
using SimpleBookingApp.Persistence.Context;
using SimpleBookingApp.Persistence.DBSeed;
using SimpleBookingApp.Persistence.Repositories;

namespace SimpleBookingApp.Persistence
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            // Scoped Services
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IEmailService, EmailManager>();

            // Logger için global handler'ı ekleyelim
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AcceptanceHandler<,>));
        }
        // Seeder
        public static void UseDatabaseSeeder(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            DatabaseSeeder.Seed(context);
        }
    }
}