using CloudSalesSystem.Models;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CloudSalesSystem.HelperClasses
{
    public static class MockHttpClient
    {
        public static void AddMockHttpClient(this IServiceCollection services)
        {
            CCPSoftware[] response = [
               new CCPSoftware{ Id = 1, Name = "Microsoft Office" },
               new CCPSoftware{ Id = 2, Name = "Microsoft Windows" },
               new CCPSoftware{ Id = 3, Name = "Microsoft Teams" },
               new CCPSoftware{ Id = 4, Name = "Microsoft Viva" },
               new CCPSoftware{ Id = 5, Name = "Microsoft Visual Studio" }];


            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri("https://www.ccp.org/products/list")),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(response))
        });
            foreach (var item in response)
            {
                var softwarePurchaseResponse = new SoftwarePurchaseResponse()
                {
                    Message = $"{item.Name} purchase Successfull",
                    Expiry = DateTime.Now.AddDays(90)
                };

                var uriString = $"https://www.css.org/services/{item.Id}/purchase";
                handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri(uriString)),
            ItExpr.IsAny<CancellationToken>()
             )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(softwarePurchaseResponse))
                });
            }

            services.AddHttpClient("FakeData").ConfigurePrimaryHttpMessageHandler(() => handlerMock.Object);
        }

    }
}