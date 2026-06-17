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

        public async Task<bool> CreateClientAsync(Client clientModel)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PostAsJsonAsync(
                        "api/clients",
                        clientModel);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateClientAsync(
            int id,
            Client clientModel)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PutAsJsonAsync(
                        $"api/clients/{id}",
                        clientModel);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.DeleteAsync(
                        $"api/clients/{id}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsApiAvailableAsync()
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.GetAsync("api/clients");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}