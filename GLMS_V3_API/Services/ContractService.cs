using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using GLMS_V3.API.Interfaces;

namespace GLMS_V3.API.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;

        public ContractService(
            IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Contract>> GetAllAsync(
            ContractStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            return await _repository.GetAllAsync(
                status,
                startDate,
                endDate);
        }

        public async Task<Contract?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Contract contract)
        {
            await _repository.AddAsync(contract);
        }

        public async Task UpdateAsync(Contract contract)
        {
            await _repository.UpdateAsync(contract);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateStatusAsync(int id, ContractStatus status)
        {
            await _repository.UpdateStatusAsync(id, status);
        }
    }
}