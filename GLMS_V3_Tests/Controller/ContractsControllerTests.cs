using GLMS.Controllers;
using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;

namespace GLMS_V3.Tests.Controller
{
    public class ContractsControllerTests
    {
        [Fact]
        public async Task Create_ShouldFail_WhenUploadedFileIsNotPdf()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var environmentMock = new Mock<IWebHostEnvironment>();

            environmentMock
                .Setup(e => e.WebRootPath)
                .Returns("C:\\Temp");

            var contractService = new ContractService(context);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var apiService = new ContractApiService(
                    httpClientFactoryMock.Object);

            var clientApiService = new ClientApiService(
                 httpClientFactoryMock.Object);

            var controller = new ContractsController(
                environmentMock.Object,
                apiService,
                clientApiService);

            var contract = new Contract
            {
                ClientId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Status = ContractStatus.Active,
                ServiceLevel = ServiceLevel.Standard
            };

            var content = "Fake file content";
            var fileName = "test.txt";

            var stream = new MemoryStream(
                Encoding.UTF8.GetBytes(content));

            IFormFile file = new FormFile(
                stream,
                0,
                stream.Length,
                "agreementFile",
                fileName);

            // Act
            var result = await controller.Create(contract, file);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.False(controller.ModelState.IsValid);

            Assert.Contains(
                controller.ModelState.Values
                    .SelectMany(v => v.Errors),
                e => e.ErrorMessage == "Only PDF files are allowed.");
        }

        [Fact]
        public async Task Create_ShouldPass_WhenUploadedFileIsPdf()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var uploadPath = Path.Combine(
            Path.GetTempPath(),
            "uploads",
            "contracts");

            Directory.CreateDirectory(uploadPath);

            var environmentMock = new Mock<IWebHostEnvironment>();

            environmentMock
                .Setup(e => e.WebRootPath)
                .Returns(Path.GetTempPath());

            var contractService = new ContractService(context);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var apiService = new ContractApiService(
                    httpClientFactoryMock.Object);


            var clientApiService = new ClientApiService(
                 httpClientFactoryMock.Object);

            var controller = new ContractsController(
                environmentMock.Object,
                apiService,
                clientApiService);

            var contract = new Contract
            {
                ClientId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Status = ContractStatus.Active,
                ServiceLevel = ServiceLevel.Premium
            };

            var content = "Fake PDF content";
            var fileName = "agreement.pdf";

            var stream = new MemoryStream(
                Encoding.UTF8.GetBytes(content));

            IFormFile file = new FormFile(
                stream,
                0,
                stream.Length,
                "agreementFile",
                fileName);

            // Act
            var result = await controller.Create(contract, file);

            // Assert
            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);

            Assert.True(controller.ModelState.IsValid);
        }
    }
}
