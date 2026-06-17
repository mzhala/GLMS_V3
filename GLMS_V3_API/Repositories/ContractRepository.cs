using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GLMS_V3.API.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> GetAllAsync(
            ContractStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Contract contract)
        {
            _context.Contracts.Add(contract);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contract contract)
        {
            _context.Update(contract);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contract =
                await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                _context.Contracts.Remove(contract);

                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateStatusAsync(int id, ContractStatus status)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract != null)
            {
                contract.Status = status;

                await _context.SaveChangesAsync();
            }
        }

    }
}