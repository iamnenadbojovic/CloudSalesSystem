using Castle.Core.Resource;
using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CloudSalesSystem.Services.CCPService
{

    public class CCPService(CloudSalesSystemDbContext cloudSalesSystemDbContext, IHttpClientFactory httpClientFactory)
    {
        public async Task<string[]> SoftwareServices()
        {
            var client = httpClientFactory.CreateClient("FakeData");
            var softwareServices = await client.GetFromJsonAsync<string[]>(
                "www.ccp.org/products/list",
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );
            return softwareServices;
        }

        public async Task<HttpResponseMessage> OrderSoftware(Guid accountId, Software softwareService)
        {
            var client = httpClientFactory.CreateClient("FakeData");
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId, softwareService }, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);
            var httpResponse = await client.PostAsync("/api/items", json);
            var accountEntry = await cloudSalesSystemDbContext.Accounts.SingleAsync(a => a.CustomerEntry.Id == accountId);
            accountEntry.SoftwareEntries.Add(softwareService);
            await cloudSalesSystemDbContext.SaveChangesAsync();
            return httpResponse;
        }
    }

}
