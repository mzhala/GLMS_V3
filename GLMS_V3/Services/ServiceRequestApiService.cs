using GLMS.Models;
using System.Net.Http.Json;

namespace GLMS.Services
{
    public class ServiceRequestApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServiceRequestApiService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var client =
                _httpClientFactory.CreateClient("GLMSApi");

            return await client.GetFromJsonAsync<List<ServiceRequest>>(
                "api/servicerequests")
                ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?> GetServiceRequestAsync(int id)
        {
            var client =
                _httpClientFactory.CreateClient("GLMSApi");

            return await client.GetFromJsonAsync<ServiceRequest>(
                $"api/servicerequests/{id}");
        }
    }
}