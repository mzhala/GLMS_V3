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
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                return await client.GetFromJsonAsync<List<ServiceRequest>>(
                    "api/servicerequests")
                    ?? new List<ServiceRequest>();
            }
            catch
            {
                return new List<ServiceRequest>();
            }
        }

        public async Task<ServiceRequest?> GetServiceRequestAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                return await client.GetFromJsonAsync<ServiceRequest>(
                    $"api/servicerequests/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateServiceRequestAsync(
            ServiceRequest serviceRequest)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PostAsJsonAsync(
                        "api/servicerequests",
                        serviceRequest);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateServiceRequestAsync(
            int id,
            ServiceRequest serviceRequest)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PutAsJsonAsync(
                        $"api/servicerequests/{id}",
                        serviceRequest);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.DeleteAsync(
                        $"api/servicerequests/{id}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}