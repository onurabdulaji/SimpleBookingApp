using SimpleBookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsByResourceIdAsync(int resourceId);
        Task<bool> IsBookingConflictAsync(int resourceId, DateTime startTime, DateTime endTime, int requestedQuantity);

    }
}
