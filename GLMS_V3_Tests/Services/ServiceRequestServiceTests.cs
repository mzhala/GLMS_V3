using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;

namespace GLMS_V3.Tests.Services
{
    public class ServiceRequestServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldFail_WhenContractIsExpired()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var contract = new Contract
            {
                Id = 1,
                ClientId = 1,
                StartDate = DateTime.Now.AddMonths(-2),
                EndDate = DateTime.Now.AddDays(-1),
                Status = ContractStatus.Expired,
                ServiceLevel = ServiceLevel.Standard
            };

            context.Contracts.Add(contract);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var currencyService =
                new CurrencyService(new HttpClient());

            var service = new ServiceRequestService(
                context,
                currencyService);

            var request = new ServiceRequest
            {
                ContractId = 1,
                Description = "Test Request",
                CostUSD = 100,
                Status = ServiceRequestStatus.Pending
            };

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.False(result.Success);

            Assert.Equal(
                "Cannot create request for expired contract.",
                result.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldPass_WhenContractIsActive()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var contract = new Contract
            {
                Id = 1,
                ClientId = 1,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(5),
                Status = ContractStatus.Active,
                ServiceLevel = ServiceLevel.Standard
            };

            context.Contracts.Add(contract);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var currencyService =
                new CurrencyService(new HttpClient());

            var service = new ServiceRequestService(
                context,
                currencyService);

            var request = new ServiceRequest
            {
                ContractId = 1,
                Description = "Valid Request",
                CostUSD = 100,
                Status = ServiceRequestStatus.Pending
            };

            // Act
            var result = await service.CreateAsync(request);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateAsync_ShouldFail_WhenPendingRequestMovesDirectlyToCompleted()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var contract = new Contract
            {
                Id = 1,
                ClientId = 1,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(5),
                Status = ContractStatus.Active,
                ServiceLevel = ServiceLevel.Premium
            };

            context.Contracts.Add(contract);

            var existingRequest = new ServiceRequest
            {
                Id = 1,
                ContractId = 1,
                Description = "Existing Request",
                CostUSD = 100,
                CostZAR = 1800,
                Status = ServiceRequestStatus.Pending
            };

            context.ServiceRequests.Add(existingRequest);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var currencyService =
                new CurrencyService(new HttpClient());

            var service = new ServiceRequestService(
                context,
                currencyService);

            var updatedRequest = new ServiceRequest
            {
                Id = 1,
                ContractId = 1,
                Description = "Updated Request",
                CostUSD = 100,
                Status = ServiceRequestStatus.Completed
            };

            // Act
            var result = await service.UpdateAsync(updatedRequest);

            // Assert
            Assert.False(result.Success);

            Assert.Equal(
                "Pending requests can only move to In Progress or Cancelled.",
                result.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldPass_WhenPendingMovesToInProgress()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var contract = new Contract
            {
                Id = 1,
                ClientId = 1,
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(5),
                Status = ContractStatus.Active,
                ServiceLevel = ServiceLevel.Standard
            };

            context.Contracts.Add(contract);

            var existingRequest = new ServiceRequest
            {
                Id = 1,
                ContractId = 1,
                Description = "Existing Request",
                CostUSD = 100,
                CostZAR = 1800,
                Status = ServiceRequestStatus.Pending
            };

            context.ServiceRequests.Add(existingRequest);

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            var currencyService =
                new CurrencyService(new HttpClient());

            var service = new ServiceRequestService(
                context,
                currencyService);

            var updatedRequest = new ServiceRequest
            {
                Id = 1,
                ContractId = 1,
                Description = "Updated Request",
                CostUSD = 100,
                Status = ServiceRequestStatus.InProgress
            };

            // Act
            var result = await service.UpdateAsync(updatedRequest);

            // Assert
            Assert.True(result.Success);
        }
    }
}