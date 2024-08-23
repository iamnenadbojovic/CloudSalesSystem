using CloudSalesSystem.Models;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CloudSalesSystem.HelperClasses
{
    public static class MockHttpClient
    {
        /// <summary>
        /// Provides mock hhtp client to simulate CCP service
        /// </summary>
        /// <param name="services">ServiceCollection object</param>
        public static void AddMockHttpClient(this IServiceCollection services)
        {
            CCPSoftware[] response = [
               new CCPSoftware{ Id = 1, Name = "Microsoft Office" },
               new CCPSoftware{ Id = 2, Name = "Microsoft Windows" },
               new CCPSoftware{ Id = 3, Name = "Microsoft Teams" },
               new CCPSoftware{ Id = 4, Name = "Microsoft Viva" },
               new CCPSoftware{ Id = 5, Name = "Microsoft Visual Studio" }];

            Func<Task<HttpResponseMessage>> productsResponse = () => Task.FromResult(
              new HttpResponseMessage()
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(response))
              });

            const string link = "https://www.ccp.org";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{link}/services/list")),
                ItExpr.IsAny<CancellationToken>()
            ).Returns(() => productsResponse());


            var purchaseSoftwareResponse = new OrderSoftwareLicenceResponse()
            {
                Message = $"Purchase Successfull",
                Expiry = DateTime.Now.AddMonths(1)
            };
            Func<Task<HttpResponseMessage>> purchaseResponse = () => Task.FromResult(
              new HttpResponseMessage()
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(JsonSerializer.Serialize(purchaseSoftwareResponse))
              });
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("order")),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(() => purchaseResponse());

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
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri!.ToString().Contains("cancel") ||
                x.RequestUri!.ToString().Contains("extend")),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(() => genericResponse());


            services.AddHttpClient("FakeData").ConfigurePrimaryHttpMessageHandler(() => handlerMock.Object);
        }
    }
}