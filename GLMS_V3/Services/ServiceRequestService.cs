using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using Microsoft.EntityFrameworkCore;
using GLMS.Models.Enums;

namespace GLMS.Services
{
    public class ServiceRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrencyService _currencyService;

        public ServiceRequestService(
        ApplicationDbContext context,
        CurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequests
                .Include(s => s.Contract)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _context.ServiceRequests
                .Include(s => s.Contract)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ServiceResult> CreateAsync(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == serviceRequest.ContractId);

            if (contract == null)
            {
                return ServiceResult.Fail("Contract not found.");
            }

            if (contract.EndDate < DateTime.Now)
            {
                return ServiceResult.Fail("Cannot create request for expired contract.");
            }

            if (contract.Status != Models.Enums.ContractStatus.Active)
            {
                return ServiceResult.Fail("Contract is not active.");
            }

            var convertedAmount = await _currencyService.ConvertUsdToZar(serviceRequest.CostUSD);

            if (convertedAmount == null)
            {
                return ServiceResult.Fail(
                    "Unable to retrieve exchange rate.");
            }

            serviceRequest.CostZAR =
                Math.Round(convertedAmount.Value, 2);

            _context.ServiceRequests.Add(serviceRequest);

            await _context.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> UpdateAsync(ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.Id == serviceRequest.ContractId);

            if (contract == null)
            {
                return ServiceResult.Fail("Contract not found.");
            }

            if (contract.EndDate < DateTime.Now)
            {
                return ServiceResult.Fail("Cannot edit request for expired contract.");
            }

            if (contract.Status != Models.Enums.ContractStatus.Active)
            {
                return ServiceResult.Fail("Contract is not active.");
            }

            var existingRequest = await _context.ServiceRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == serviceRequest.Id);

            if (existingRequest == null)
            {
                return ServiceResult.Fail("Service request not found.");
            }

            // Completed requests cannot change
            if (existingRequest.Status == ServiceRequestStatus.Completed)
            {
                return ServiceResult.Fail("Completed requests cannot be modified.");
            }

            // Cancelled requests cannot change
            if (existingRequest.Status == ServiceRequestStatus.Cancelled)
            {
                return ServiceResult.Fail("Cancelled requests cannot be modified.");
            }

            // Pending rules
            if (existingRequest.Status == ServiceRequestStatus.Pending)
            {
                var allowedStatuses = new[]
                {
                ServiceRequestStatus.Pending,
                ServiceRequestStatus.InProgress,
                ServiceRequestStatus.Cancelled
                };

                if (!allowedStatuses.Contains(serviceRequest.Status))
                {
                    return ServiceResult.Fail(
                        "Pending requests can only move to In Progress or Cancelled.");
                }
            }

            // InProgress rules
            if (existingRequest.Status == ServiceRequestStatus.InProgress)
            {
                var allowedStatuses = new[]
                {
                ServiceRequestStatus.InProgress,
                ServiceRequestStatus.Completed,
                ServiceRequestStatus.Cancelled
                };

                if (!allowedStatuses.Contains(serviceRequest.Status))
                {
                    return ServiceResult.Fail(
                        "In Progress requests can only move to Completed or Cancelled.");
                }
            }

            var convertedAmount =
            await _currencyService.ConvertUsdToZar(
            serviceRequest.CostUSD);

            if (convertedAmount == null)
            {
                return ServiceResult.Fail(
                    "Unable to retrieve exchange rate.");
            }

            serviceRequest.CostZAR =
                Math.Round(convertedAmount.Value, 2);

            _context.Entry(serviceRequest).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task DeleteAsync(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);

            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id)
        {
            return _context.ServiceRequests.Any(s => s.Id == id);
        }
    }
}