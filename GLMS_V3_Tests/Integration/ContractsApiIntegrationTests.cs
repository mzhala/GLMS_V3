using GLMS.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace GLMS_V3.Tests.Integration
{
    public class ContractsApiIntegrationTests
    {
        // Integration Test:
        // Verify that the Contracts API endpoint is reachable
        // and returns a successful HTTP response with data.
        [Fact]
        public async Task GetContracts_ShouldReturn200()
        {
            // API must be running
            using var client = new HttpClient();

            var response =
                await client.GetAsync(
                    "https://localhost:7277/api/contracts");

            Assert.True(
                response.IsSuccessStatusCode);

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(
                string.IsNullOrEmpty(json));
        }

        // Integration Test:
        // Verify that a Client can be created through the API
        // and then retrieved successfully, ensuring data integrity.
        [Fact]
        public async Task CreateClient_ThenReadClient_ShouldMatch()
        {
            using var client = new HttpClient();

            var newClient = new Client
            {
                Name = $"IntegrationTest_{Guid.NewGuid()}",
                ContactDetails = "0821234567",
                Region = "Durban"
            };

            // Create
            var postResponse =
                await client.PostAsJsonAsync(
                    "https://localhost:7277/api/clients",
                    newClient);

            Assert.True(postResponse.IsSuccessStatusCode);

            var createdClient =
                await postResponse.Content
                    .ReadFromJsonAsync<Client>();

            Assert.NotNull(createdClient);

            // Read
            var getResponse =
                await client.GetAsync(
                    $"https://localhost:7277/api/clients/{createdClient!.Id}");

            Assert.True(getResponse.IsSuccessStatusCode);

            var retrievedClient =
                await getResponse.Content
                    .ReadFromJsonAsync<Client>();

            Assert.NotNull(retrievedClient);

            Assert.Equal(
                createdClient.Name,
                retrievedClient!.Name);
        }

        // Integration Test:
        // Verify that the Clients API endpoint is reachable
        // and returns a successful HTTP response with data.
        [Fact]
        public async Task GetClients_ShouldReturn200()
        {
            using var client = new HttpClient();

            var response =
                await client.GetAsync(
                    "https://localhost:7277/api/clients");

            Assert.True(response.IsSuccessStatusCode);

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrEmpty(json));
        }

        // Integration Test:
        // Verify that the Service Requests API endpoint is reachable
        // and returns a successful HTTP response with data.
        [Fact]
        public async Task GetServiceRequests_ShouldReturn200()
        {
            using var client = new HttpClient();

            var response =
                await client.GetAsync(
                    "https://localhost:7277/api/servicerequests");

            Assert.True(response.IsSuccessStatusCode);

            var json =
                await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrEmpty(json));
        }

        // Integration Test:
        // Verify that a specific Contract can be retrieved
        // by its identifier from the API.
        [Fact]
        public async Task GetContractById_ShouldReturnContract()
        {
            using var client = new HttpClient();

            var response =
                await client.GetAsync(
                    "https://localhost:7277/api/contracts/1");

            Assert.True(response.IsSuccessStatusCode);

            var contract =
                await response.Content
                    .ReadFromJsonAsync<Contract>();

            Assert.NotNull(contract);
        }

        // Integration Test:
        // Verify that a Client can be created, updated,
        // and then retrieved with the updated values.
        [Fact]
        public async Task CreateClient_UpdateClient_ShouldPersistChanges()
        {
            using var client = new HttpClient();

            var newClient = new Client
            {
                Name = $"IntegrationTest_{Guid.NewGuid()}",
                ContactDetails = "0821234567",
                Region = "Durban"
            };

            var createResponse =
                await client.PostAsJsonAsync(
                    "https://localhost:7277/api/clients",
                    newClient);

            createResponse.EnsureSuccessStatusCode();

            var createdClient =
                await createResponse.Content
                    .ReadFromJsonAsync<Client>();

            createdClient!.Region = "Cape Town";

            var updateResponse =
                await client.PutAsJsonAsync(
                    $"https://localhost:7277/api/clients/{createdClient.Id}",
                    createdClient);

            Assert.True(updateResponse.IsSuccessStatusCode);

            var readResponse =
                await client.GetAsync(
                    $"https://localhost:7277/api/clients/{createdClient.Id}");

            var updatedClient =
                await readResponse.Content
                    .ReadFromJsonAsync<Client>();

            Assert.Equal(
                "Cape Town",
                updatedClient!.Region);
        }
    }
}