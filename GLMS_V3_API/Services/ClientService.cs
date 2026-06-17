using GLMS.Models;
using GLMS.Models.Enums;
using GLMS_V3.API.Interfaces;
using GLMS_V3.API.Interfaces;
using GLMS_V3_API.Interfaces;

namespace GLMS_V3.API.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(
            IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Client Client)
        {
            await _repository.AddAsync(Client);
        }

        public async Task UpdateAsync(Client Client)
        {
            await _repository.UpdateAsync(Client);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}