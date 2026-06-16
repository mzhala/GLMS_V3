using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Services
{
    public class ContractService
    {
        private readonly ApplicationDbContext _context;

        public ContractService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contract>> GetAllAsync()
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .ToListAsync();
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ServiceResult> CreateAsync(Contract contract)
        {
            if (contract.EndDate < contract.StartDate)
            {
                return ServiceResult.Fail(
                    "End date cannot be before start date.");
            }

            if (contract.Status == ContractStatus.Active &&
                contract.EndDate < DateTime.Now)
            {
                return ServiceResult.Fail(
                    "Active contracts cannot already be expired.");
            }

            contract.Status = DetermineContractStatus(contract);
            _context.Contracts.Add(contract);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        private ContractStatus DetermineContractStatus(Contract contract)
        {
            if (contract.Status == ContractStatus.OnHold)
            {
                return ContractStatus.OnHold;
            }

            var today = DateTime.Now;

            if (today < contract.StartDate)
            {
                return ContractStatus.Draft;
            }

            if (today > contract.EndDate)
            {
                return ContractStatus.Expired;
            }

            return ContractStatus.Active;
        }

        public bool Exists(int id)
        {
            return _context.Contracts.Any(c => c.Id == id);
        }
    }
}