using SimpleBookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Application.Interfaces
{
    public interface IResourceRepository
    {
        Task<Resource> GetByIdAsync(int id);
        Task<IEnumerable<Resource>> GetAllAsync();
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(Resource resource);
    }
}
