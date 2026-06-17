using GLMS.Models;
using GLMS.Models.Enums;

namespace GLMS_V3_API.Interfaces
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllAsync();

        Task<ServiceRequest?> GetByIdAsync(int id);

        Task AddAsync(ServiceRequest ServiceRequest);

        Task UpdateAsync(ServiceRequest ServiceRequest);

        Task DeleteAsync(int id);
    }
}
