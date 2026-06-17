using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using GLMS_V3.API.Interfaces;
using GLMS_V3_API.Interfaces;

namespace GLMS_V3.API.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IServiceRequestRepository _repository;

        public ServiceRequestService(
            IServiceRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ServiceRequest?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(ServiceRequest ServiceRequest)
        {
            await _repository.AddAsync(ServiceRequest);
        }

        public async Task UpdateAsync(ServiceRequest ServiceRequest)
        {
            await _repository.UpdateAsync(ServiceRequest);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}