using GLMS.Models;
using System.Net.Http.Json;

namespace GLMS.Services
{
    public class ClientApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientApiService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                return await client.GetFromJsonAsync<List<Client>>(
                    "api/clients")
                    ?? new List<Client>();
            }
            catch
            {
                return new List<Client>();
            }
        }

        public async Task<Client?> GetClientAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                return await client.GetFromJsonAsync<Client>(
                    $"api/clients/{id}");
            }
            catch
            {
                return null;
            }
        }
    }
}