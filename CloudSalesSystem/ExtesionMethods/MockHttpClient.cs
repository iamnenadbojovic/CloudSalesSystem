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

            const string link = "https://www.ccp.org";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{link}/products/list")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(response))
            }
            );
            foreach (var item in response)
            {
                var softwarePurchaseResponse = new SoftwarePurchaseResponse()
                {
                    Message = $"{item.Name} purchase Successfull",
                    Expiry = DateTime.Now.AddMonths(1)
                };

                var uriString = $"{link}/{item.Id}/purchase";
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

                var cancelUriString = $"{link}/{item.Id}/cancel";
                handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri(cancelUriString)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(softwarePurchaseResponse))
                });

                var extendUriString = $"{link}/{item.Id}/extend";
                handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri(extendUriString)),
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