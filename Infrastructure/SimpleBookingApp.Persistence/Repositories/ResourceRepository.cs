using Microsoft.EntityFrameworkCore;
using SimpleBookingApp.Application.Interfaces;
using SimpleBookingApp.Domain.Entities;
using SimpleBookingApp.Persistence.Context;


namespace SimpleBookingApp.Persistence.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext _context;

        public ResourceRepository(AppDbContext appContext)
        {
            _context = appContext;
        }

        public async Task AddAsync(Resource resource)
        {
            await _context.Resources.AddAsync(resource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resource resource)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Resource>> GetAllAsync()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<Resource> GetByIdAsync(int id)
        {
            return await _context.Resources
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }
    }
}
