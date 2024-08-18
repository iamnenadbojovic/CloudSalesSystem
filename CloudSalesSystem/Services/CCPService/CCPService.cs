using CloudSalesSystem.DBContext;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CloudSalesSystem.Services.CCPService
{

    public class CCPService(
        CloudSalesSystemDbContext cloudSalesSystemDbContext, 
        IHttpClientFactory httpClientFactory) : ICCPService
    {
        public async Task<CCPSoftware[]> SoftwareServices()
        {
            var client = httpClientFactory.CreateClient("FakeData");
            var softwareServices = await client.GetFromJsonAsync<CCPSoftware[]>(
                "https://www.ccp.org/products/list",
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );
            return softwareServices;
        }

        public async Task<HttpResponseMessage> OrderSoftware(Guid accountId, int softwareId)
        {
            var services  = await SoftwareServices();
            var softwareCheck = services.FirstOrDefault(a => a.Id == softwareId);
            if(softwareCheck== null)
            {
                return null;
            }
            var client = httpClientFactory.CreateClient("FakeData");
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId}, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);
            // ovo treba u header
            var httpResponse = await client.PostAsync($"https://www.css.org/services/{softwareId}/purchase", json);
            string msg = await httpResponse.Content.ReadAsStringAsync();
            var softwarePurchaseResponse = JsonSerializer.Deserialize<SoftwarePurchaseResponse>(msg);
            var accountEntry = await cloudSalesSystemDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            var softwareEntry = new Software()
            {
                Name = softwareCheck.Name,
                Quantity = 1,
                ValidToDate = softwarePurchaseResponse.Expiry,
            };
            accountEntry.SoftwareEntries.Add(softwareEntry);
            await cloudSalesSystemDbContext.SaveChangesAsync();
            return httpResponse;
        }
    }

}
