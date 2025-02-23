using Microsoft.EntityFrameworkCore;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Domain.Entities;
using SimpleBookingApp.Persistence.Context;

namespace SimpleBookingApp.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Resource)  // İlgili resource'ı da dahil edebiliriz
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resource)  // Resource'ları da dahil edebiliriz
                .ToListAsync();
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Booking>> GetBookingsByResourceIdAsync(int resourceId)
        {
            return await _context.Bookings
                .Where(b => b.ResourceId == resourceId)
                .ToListAsync();
        }

        public async Task<bool> IsResourceAvailableAsync(int resourceId, DateTime startTime, DateTime endTime, int requestedQuantity)
        {
            // Kaynağın mevcut kapasitesini alın
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource == null)
            {
                throw new InvalidOperationException("Resource not found.");
            }

            // Mevcut rezervasyonları kontrol et
            var existingBookings = await _context.Bookings
                .Where(b => b.ResourceId == resourceId &&
                            b.DateFrom < endTime &&
                            b.DateTo > startTime)
                .ToListAsync();

            // Kaynakla ilgili toplam rezervasyonu hesaplıyoruz
            var totalQuantityBooked = existingBookings
                .Sum(b => b.BookedQuantity);

            // Eğer toplam rezervasyonla birlikte kaynak kapasitesini aşarsa, hata fırlat
            if (totalQuantityBooked + requestedQuantity > resource.Quantity)
            {
                throw new InvalidOperationException($"The requested quantity exceeds the available quantity of the resource. Maximum available quantity is {resource.Quantity - totalQuantityBooked}.");
            }

            // Eğer kapasite yetersizse, false döndürme
            return true;
        }
    }
}
