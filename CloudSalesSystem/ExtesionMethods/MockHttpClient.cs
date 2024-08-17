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
            string[] response = ["Microsoft Office", "Microsoft Windows", "Microsoft Teams", "Microsoft Viva", "Microsoft Visual Studio"];

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri("www.css.org/services")),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(response))
        });
            foreach (var item in response)
            {
                var uriString = $"www.css.org/services/{item}/purchase";
                handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri(uriString)),
            ItExpr.IsAny<CancellationToken>()
        )
        .ReturnsAsync(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("Licence Purchased")
        });
            }

            services.AddHttpClient("FakeData").ConfigurePrimaryHttpMessageHandler(() => handlerMock.Object);
        }
    }
}