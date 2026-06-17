using GLMS.Models;
using GLMS.Services;
using GLMS.Models.Enums;
using GLMS_V3_API.Interfaces;


namespace GLMS_V3.API.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _repository;
        private readonly CurrencyService _currencyService;

        public ServiceRequestService(
            IServiceRequestRepository repository,
            CurrencyService currencyService)
        {
            _repository = repository;
            _currencyService = currencyService;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(ServiceRequest serviceRequest)
        {
            var convertedAmount =
                await _currencyService.ConvertUsdToZar(
                    serviceRequest.CostUSD);

            if (convertedAmount != null)
            {
                serviceRequest.CostZAR =
                    Math.Round(convertedAmount.Value, 2);
            }

            await _repository.AddAsync(serviceRequest);
        }

        public async Task UpdateAsync(ServiceRequest serviceRequest)
        {
            var convertedAmount =
                await _currencyService.ConvertUsdToZar(
                    serviceRequest.CostUSD);

            if (convertedAmount != null)
            {
                serviceRequest.CostZAR =
                    Math.Round(convertedAmount.Value, 2);
            }

            await _repository.UpdateAsync(serviceRequest);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}