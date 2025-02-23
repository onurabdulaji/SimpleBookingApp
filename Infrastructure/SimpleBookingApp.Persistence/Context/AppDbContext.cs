using Microsoft.EntityFrameworkCore;
using SimpleBookingApp.Domain.Entities;
using SimpleBookingApp.Persistence.Configurations;


namespace SimpleBookingApp.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BookingConfiguration());
            builder.ApplyConfiguration(new ResourceConfiguration());

        }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
