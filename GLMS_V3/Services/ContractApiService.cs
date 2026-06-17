using GLMS.Models;
using GLMS.Models.Enums;
using System.Net.Http.Json;

namespace GLMS.Services
{
    public class ContractApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ContractApiService(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Contract>> GetContractsAsync(
            ContractStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var queryParams = new List<string>();

                if (status.HasValue)
                    queryParams.Add($"status={status}");

                if (startDate.HasValue)
                    queryParams.Add(
                        $"startDate={startDate.Value:yyyy-MM-dd}");

                if (endDate.HasValue)
                    queryParams.Add(
                        $"endDate={endDate.Value:yyyy-MM-dd}");

                var url = "api/contracts";

                if (queryParams.Any())
                {
                    url += "?" + string.Join("&", queryParams);
                }

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Contract>();
                }

                return await response.Content
                    .ReadFromJsonAsync<List<Contract>>()
                    ?? new List<Contract>();
            }
            catch
            {
                return new List<Contract>();
            }
        }

        public async Task<Contract?> GetContractAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                return await client.GetFromJsonAsync<Contract>(
                    $"api/contracts/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateContractAsync(Contract contract)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PostAsJsonAsync(
                        "api/contracts",
                        contract);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateContractAsync(
            int id,
            Contract contract)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.PutAsJsonAsync(
                        $"api/contracts/{id}",
                        contract);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            try
            {
                var client =
                    _httpClientFactory.CreateClient("GLMSApi");

                var response =
                    await client.DeleteAsync(
                        $"api/contracts/{id}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}