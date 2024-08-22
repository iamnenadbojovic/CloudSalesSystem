using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using CloudSalesSystem.Services.CCPService;
using Moq;
using FluentAssertions;
using Moq.EntityFrameworkCore;

namespace CloudSalesSystemTests
{
    public class CustomerServiceTests: BaseTests
    {

        #region  SoftwareServices Tests

        [Fact]
        async Task CustomerAccounts_RetursList()
        {
            // Arrange
            var mockContext = new Mock<CloudSalesSystemDbContext>(options);

            var accounts = new List<Account>() { account };
            var softwares = new List<Software>() { softwareEntry };

            mockContext.Setup(m => m.Accounts).ReturnsDbSet(accounts);
            mockContext.Setup(m => m.Softwares).ReturnsDbSet(softwares);


            // Act
            var ccpService = new CustomerService(mockContext.Object);
            var result = await ccpService.CustomerAccounts(customer.Id);


            // Assert
            var expected = new List<Account> { account };
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        async Task CustomerAccounts_RetursEmptyList()
        {
            // Arrange
            var customerEmptyAccount = new Guid("1D6ABF8E-6A2E-4606-9714-1175B7B4DE73");
            var mockContext = new Mock<CloudSalesSystemDbContext>(options);

            var accounts = new List<Account>() { account };
            var softwares = new List<Software>() { softwareEntry };

            mockContext.Setup(m => m.Accounts).ReturnsDbSet(accounts);
            mockContext.Setup(m => m.Softwares).ReturnsDbSet(softwares);

            var ccpService = new CustomerService(mockContext.Object);

            // Act
            var result = await ccpService.CustomerAccounts(customer.Id);

            // Assert
            var expected = new List<Account> { account };
            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region  UpdateLicenceQuantity Tests

        [Theory]
        [InlineData("936ac57d-ee15-4156-9104-2799ac5d2d07", "936ac57d-eeee-4156-9104-2799ac5d2d07", 404)]
        [InlineData("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3", "2D6ABF8E-6A2E-4606-9714-1175B7B4DE73", 200)]
        async Task UpdateLicenceQuantity_Returns_StatusCode200_StatusCode404(string customerId, string softwareId, int statusCode)
        {
            // Arrange
            var mockContext = new Mock<CloudSalesSystemDbContext>(options);

            var accounts = new List<Account>() { account };
            var softwares = new List<Software>() { softwareEntry };

            mockContext.Setup(m => m.Accounts).ReturnsDbSet(accounts);
            mockContext.Setup(m => m.Softwares).ReturnsDbSet(softwares);


            // Act
            var ccpService = new CustomerService(mockContext.Object);
            var result = await ccpService.UpdateLicenceQuantity(new Guid(customerId), new Guid(softwareId), 3);


            // Assert
            Assert.Equal(statusCode, (int)result);
        }

        #endregion
    }
}