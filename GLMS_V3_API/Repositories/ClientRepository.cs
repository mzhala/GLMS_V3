using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using GLMS_V3_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GLMS_V3.API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Client Client)
        {
            _context.Clients.Add(Client);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Client Client)
        {
            _context.Update(Client);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Client =
                await _context.Clients.FindAsync(id);

            if (Client != null)
            {
                _context.Clients.Remove(Client);

                await _context.SaveChangesAsync();
            }
        }

    }
}