using GLMS.Models;
using System.ComponentModel.DataAnnotations;

namespace GLMS_V3.Tests.Models
{
    public class ClientValidationTests
    {
        [Fact]
        public void Client_ShouldFail_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            var client = new Client
            {
                Name = "Test Client",
                ContactDetails = "INVALID_PHONE",
                Region = "Gauteng"
            };

            var validationResults =
                new List<ValidationResult>();

            var context =
                new ValidationContext(client);

            // Act
            var isValid = Validator.TryValidateObject(
                client,
                context,
                validationResults,
                true);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Client_ShouldPass_WhenPhoneNumberIsValid()
        {
            // Arrange
            var client = new Client
            {
                Name = "Test Client",
                ContactDetails = "0821234567",
                Region = "Gauteng"
            };

            var validationResults =
                new List<ValidationResult>();

            var context =
                new ValidationContext(client);

            // Act
            var isValid = Validator.TryValidateObject(
                client,
                context,
                validationResults,
                true);

            // Assert
            Assert.True(isValid);
        }
    }
}