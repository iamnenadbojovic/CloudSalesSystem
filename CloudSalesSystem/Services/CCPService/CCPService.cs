using CloudSalesSystem.DBContext;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
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
                "https://www.ccp.org/services/list",
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );
            return softwareServices;
        }

        public async Task<HttpStatusCode> OrderSoftware(Guid customerId, Guid accountId, int softwareId)
        {
            var services = await SoftwareServices();
            var softwareListCheck = Array.Find(services, (a => a.Id == softwareId));
            if (softwareListCheck == null)
            {
                return HttpStatusCode.Forbidden;

            }
 
            var accountEntry = await cloudSalesSystemDbContext.Accounts.FirstOrDefaultAsync(
                a => a.Id == accountId
                && a.Customer.Id == customerId);

            if (accountEntry == null)
            {
                return HttpStatusCode.NotFound;
            }

            var softwareExists = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
                a => a.Account.Id == accountId
                && a.Account.Customer.Id == customerId && a.CCPID == softwareId);
            if(softwareExists != null)
            {
                return HttpStatusCode.Conflict;
            }


            var client = httpClientFactory.CreateClient("FakeData");
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId }, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);

            var httpResponse = await client.PostAsync($"https://www.css.org/services/{softwareId}/order", json);
            string httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return httpResponse.StatusCode;
            }

            var softwarePurchaseResponse = JsonSerializer.Deserialize<SoftwarePurchaseResponse>(httpResponseContent);

            var softwareEntry = new Software()
            {
                Name = softwareListCheck.Name!,
                Quantity = 1,
                CCPID= softwareId,
                ValidToDate = softwarePurchaseResponse!.Expiry,
            };

            accountEntry.SoftwareEntries.Add(softwareEntry);
            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }


        public async Task<HttpStatusCode> CancelSubscription(Guid customerId, Guid softwareId)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
            a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId && a.State!="Cancelled");
            if (softwareEntity == null)
            {
                return HttpStatusCode.NotFound;
            }
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId =  softwareEntity.Account.Id }, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);

            var client = httpClientFactory.CreateClient("FakeData");
            var httpResponse = await client.PostAsync($"https://www.css.org/services/{softwareId}/cancel", json);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return httpResponse.StatusCode;
            }
            softwareEntity.State = "Cancelled";

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> ExtendSoftwareLicence(Guid customerId, Guid softwareId, int days)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
            a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId && a.State!="Cancelled");
            if (softwareEntity == null)
            {
                return HttpStatusCode.NotFound;
            }
            using StringContent json = new(
              JsonSerializer.Serialize(new { accountId = softwareEntity.Account.Id }, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
              Encoding.UTF8,
              MediaTypeNames.Application.Json);

            var client = httpClientFactory.CreateClient("FakeData");
            var httpResponse = await client.PostAsync($"https://www.css.org/services/{softwareId}/extend", json);
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return httpResponse.StatusCode;
            }
            softwareEntity.ValidToDate = softwareEntity.ValidToDate.AddDays(days);

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }

}
