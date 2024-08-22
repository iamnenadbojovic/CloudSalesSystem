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
                "https://www.ccp.org/products/list",
                new JsonSerializerOptions(JsonSerializerDefaults.Web)
                );
            return softwareServices!;
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

            var client = httpClientFactory.CreateClient("FakeData");
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId }, new JsonSerializerOptions(JsonSerializerDefaults.Web)),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);

            var httpResponse = await client.PostAsync($"https://www.css.org/services/{softwareId}/purchase", json);
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
                ValidToDate = softwarePurchaseResponse!.Expiry,
            };

            accountEntry.SoftwareEntries.Add(softwareEntry);
            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<bool> UpdateLicenceQuantity(Guid customerId, Guid softwareId, int quantity)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
                a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId);

            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.Quantity = quantity;

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelSubscription(Guid customerId, Guid softwareId)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
            a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.State = "Cancelled";

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExtendSoftwareLicence(Guid customerId, Guid softwareId, int days)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
            a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.ValidToDate = softwareEntity.ValidToDate.AddDays(days);

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }
    }

}
