using GLMS.Models;
using GLMS.Models.Enums;

namespace GLMS_V3_API.Interfaces
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync();

        Task<Client?> GetByIdAsync(int id);

        Task AddAsync(Client client);

        Task UpdateAsync(Client client);

        Task DeleteAsync(int id);
    }
}
