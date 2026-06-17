using GLMS.Models;
using GLMS.Models.Enums;

namespace GLMS_V3.API.Interfaces
{
    public interface IContractRepository
    {
        Task<List<Contract>> GetAllAsync(
            ContractStatus? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Contract?> GetByIdAsync(int id);

        Task AddAsync(Contract contract);

        Task UpdateAsync(Contract contract);
        Task UpdateStatusAsync(int id, ContractStatus status);

        Task DeleteAsync(int id);
    }
}