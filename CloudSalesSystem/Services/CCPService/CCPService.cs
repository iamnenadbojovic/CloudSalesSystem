using CloudSalesSystem.DBContext;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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

        public async Task<HttpStatusCode> OrderSoftware(Guid customerId, Guid accountId, int softwareId)
        {
            var services  = await SoftwareServices();
            var softwareListCheck = Array.Find(services, (a => a.Id == softwareId));
            if(softwareListCheck== null)
            {
                return HttpStatusCode.Forbidden;
              
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
            var accountEntry = await cloudSalesSystemDbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId 
            && a.Customer.Id == customerId );
            var softwareEntry = new Software()
            {
                Name = softwareListCheck.Name!,
                Quantity = 1,
                ValidToDate = softwarePurchaseResponse!.Expiry,
            };
            if (accountEntry == null)
            {
                return HttpStatusCode.NotFound;
            }
            accountEntry.SoftwareEntries.Add(softwareEntry);
            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }

}
