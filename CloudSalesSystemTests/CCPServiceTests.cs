using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using CloudSalesSystem.Services.CCPService;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CloudSalesSystemTests
{
    public class CCPServiceTests: BaseTests
    {
        #region private fields

        private readonly string Url = "https://www.ccp.org";

        private readonly CCPSoftware[] expected = [
            new CCPSoftware{ Id = 1, Name = "Microsoft Office" },
               new CCPSoftware{ Id = 2, Name = "Microsoft Windows" },
               new CCPSoftware{ Id = 3, Name = "Microsoft Teams" },
               new CCPSoftware{ Id = 4, Name = "Microsoft Viva" },
               new CCPSoftware{ Id = 5, Name = "Microsoft Visual Studio" }];

        #endregion

        #region  SoftwareServices Tests

        [Fact]
        async Task SoftwareServices_RetursArray()
        {
            // Arrange
            Task<HttpResponseMessage> productsResponse = Task.FromResult(
             new HttpResponseMessage()
             {
                 StatusCode = HttpStatusCode.OK,
                 Content = new StringContent(JsonSerializer.Serialize(expected))
             });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{Url}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(productsResponse);

            // Act
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);
            var result = await ccpService.SoftwareServices();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        async Task SoftwareServices_ReturnsEmptyArray()
        {
            // Arrange
            CCPSoftware[] content = [];
            Task<HttpResponseMessage> productsResponse = Task.FromResult(
             new HttpResponseMessage()
             {
                 StatusCode = HttpStatusCode.NotFound,
             });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{Url}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(productsResponse);

           
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);

            // Act
            var result = await ccpService.SoftwareServices();

            // Assert
            result.Should().BeEquivalentTo(content);
        }

        #endregion

        #region OrderSoftware Tests

        [Fact]
        async Task OrderSoftware_Returns_StatusCode200()
        {
            // Arrange

            Task<HttpResponseMessage> productsResponse = Task.FromResult(
             new HttpResponseMessage()
             {
                 StatusCode = HttpStatusCode.OK,
                 Content = new StringContent(JsonSerializer.Serialize(expected))
             });
            var softwarePurchaseResponse = new SoftwarePurchaseResponse()
            {
                Message = $"Purchase Successfull",
                Expiry = DateTime.Now.AddMonths(1)
            };
            Func<Task<HttpResponseMessage>> purchaseResponse = () => Task.FromResult(
              new HttpResponseMessage()
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(softwarePurchaseResponse))
              });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{Url}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(productsResponse);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("order")),
                   ItExpr.IsAny<CancellationToken>()
               )
               .Returns(() => purchaseResponse());

         
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);
            var result = await ccpService.OrderSoftware(customer.CustomerId, account.AccountId,ccpId:1);
            var expectedResult = HttpStatusCode.OK;

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        async Task OrderSoftware_Returns_StatusCode403()
        {
            // Arrange

            var emptyResponse = new CCPSoftware[] { };
            Task<HttpResponseMessage> productsResponse = Task.FromResult(
             new HttpResponseMessage()
             {
                 StatusCode = HttpStatusCode.OK,
                 Content = new StringContent(JsonSerializer.Serialize(emptyResponse))
             });

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{Url}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(productsResponse);
            
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);
            var result = await ccpService.OrderSoftware(Guid.Empty, Guid.Empty,2);

            var expectedResult = HttpStatusCode.Forbidden;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("936ac57d-ee15-4156-9104-2799ac5d2d07", "936ac57d-eeee-4156-9104-2799ac5d2d07", 404)]
        [InlineData("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3", "7D6ABF8E-6A2E-4606-9714-1175B7B4DE73", 409)]
        async Task OrderSoftware_Returns_StatusCode404_StatusCode409(string customerId,string accountId, int statusCode)
        {
            // Arrange
            Task<HttpResponseMessage> productsResponse = Task.FromResult(
             new HttpResponseMessage()
             {
                 StatusCode = HttpStatusCode.OK,
                 Content = new StringContent(JsonSerializer.Serialize(expected))
             });
            var softwarePurchaseResponse = new SoftwarePurchaseResponse()
            {
                Message = $"Purchase Successfull",
                Expiry = DateTime.Now.AddMonths(1)
            };
            Func<Task<HttpResponseMessage>> purchaseResponse = () => Task.FromResult(
              new HttpResponseMessage()
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(softwarePurchaseResponse))
              });
 
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{Url}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(productsResponse);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("order")),
                   ItExpr.IsAny<CancellationToken>()
               )
               .Returns(() => purchaseResponse());
            
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Act
            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);
            var result = await ccpService.OrderSoftware(new Guid(customerId), new Guid(accountId), ccpId: 2);

            // Assert
            Assert.Equal(statusCode, (int)result);
        }

        #endregion

        #region CancelSubscription Tests

        [Theory]
        [InlineData("936ac57d-ee15-4156-9104-2799ac5d2d07", "936ac57d-eeee-4156-9104-2799ac5d2d07", 404)]
        [InlineData("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3", "2D6ABF8E-6A2E-4606-9714-1175B7B4DE73", 200)]
        async Task CancelSubscription_Returns_StatusCode200_StatusCode404(string customerId, string softwareId, int statusCode )
        {
            var mockContext = new Mock<CloudSalesSystemDbContext>(options);

            var customers = new List<Customer>() { customer };
            var accounts = new List<Account>() { account };
            var softwares = new List<Software>() { softwareEntry };

            mockContext.Setup(m => m.Customers).ReturnsDbSet(customers);
            mockContext.Setup(m => m.Accounts).ReturnsDbSet(accounts);
            mockContext.Setup(m => m.Softwares).ReturnsDbSet(softwares);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var genericResponseContent = new BaseResponse()
            {
                Message = $"Action Successfull",
            };

            Func<Task<HttpResponseMessage>> genericResponse = () => Task.FromResult(
                  new HttpResponseMessage()
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(JsonSerializer.Serialize(genericResponseContent))
                  });

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("cancel")),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(() => genericResponse());


            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);

            // Act
            var result = await ccpService.CancelSubscription(new Guid(customerId), new Guid(softwareId));

            // Assert
            Assert.Equal(statusCode, (int)result);
        }

        #endregion

        #region ExtendSoftwareLicence Tests

        [Theory]
        [InlineData("936ac57d-ee15-4156-9104-2799ac5d2d07", "936ac57d-eeee-4156-9104-2799ac5d2d07", 404)]
        [InlineData("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3", "2D6ABF8E-6A2E-4606-9714-1175B7B4DE73", 200)]
        async Task ExtendSoftwareLicence_Returns_StatusCode200_StatusCode404(string customerId, string softwareId, int statusCode)
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var genericResponseContent = new BaseResponse()
            {
                Message = $"Action Successfull",
            };
            Func<Task<HttpResponseMessage>> genericResponse = () => Task.FromResult(
                  new HttpResponseMessage()
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(JsonSerializer.Serialize(genericResponseContent))
                  });

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("extend")),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(() => genericResponse());

         
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient(handlerMock.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var ccpService = new CCPService(mockContext.Object, mockFactory.Object);

            // Act
            var result = await ccpService.ExtendSoftwareLicence(new Guid(customerId), new Guid(softwareId), 30);

            // Assert
            Assert.Equal(statusCode, (int)result);
        }

        #endregion
    }
}