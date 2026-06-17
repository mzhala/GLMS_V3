using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using GLMS_V3_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GLMS_V3.API.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequests.ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _context.ServiceRequests
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(ServiceRequest ServiceRequest)
        {
            _context.ServiceRequests.Add(ServiceRequest);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServiceRequest ServiceRequest)
        {
            _context.Update(ServiceRequest);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ServiceRequest =
                await _context.ServiceRequests.FindAsync(id);

            if (ServiceRequest != null)
            {
                _context.ServiceRequests.Remove(ServiceRequest);

                await _context.SaveChangesAsync();
            }
        }

    }
}