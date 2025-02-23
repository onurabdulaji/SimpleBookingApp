using SimpleBookingApp.Domain.Entities;
using SimpleBookingApp.Persistence.Context;

namespace SimpleBookingApp.Persistence.DBSeed
{
    public class DatabaseSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Resources.Any())
            {
                context.Resources.AddRange(
                    new Resource
                    {
                        Name = "Projector",
                        Quantity = 500
                    },
                    new Resource
                    {
                        Name = "Laptop",
                        Quantity = 500
                    }
                );
                context.SaveChanges();
            }

            if (!context.Bookings.Any())
            {
                context.Bookings.AddRange(
                    new Booking
                    {
                        ResourceId = 1,
                        BookedQuantity = 2,
                        DateFrom = DateTime.Now.AddDays(1),
                        DateTo = DateTime.Now.AddDays(2)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
