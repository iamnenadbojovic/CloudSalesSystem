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
        private readonly string url = "https://www.ccp.org/services";
        private readonly JsonSerializerOptions jsonOptions =  new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public async Task<CCPSoftware[]> SoftwareServices()
        {
            var client = httpClientFactory.CreateClient("FakeData");
            var response = await client.GetAsync($"{url}/list");

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }
            var softwareServices = await response.Content.ReadFromJsonAsync<CCPSoftware[]>();

            return softwareServices!;
        }
    

        public async Task<HttpStatusCode> OrderSoftware(Guid customerId, Guid accountId, int ccpId)
        {
            var services = await SoftwareServices();
            var softwareListCheck = Array.Find(services, (a => a.Id == ccpId));
            if (softwareListCheck == null)
            {
                return HttpStatusCode.Forbidden;

            }
 
            var accountEntry = await cloudSalesSystemDbContext.Accounts.FirstOrDefaultAsync(
                a => a.AccountId == accountId
                && a.Customer.CustomerId == customerId);

            if (accountEntry == null)
            {
                return HttpStatusCode.NotFound;
            }

            var softwareExists = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
                a => a.Account.AccountId == accountId
                && a.Account.Customer.CustomerId == customerId && a.CCPID == ccpId);
            if(softwareExists != null)
            {
                return HttpStatusCode.Conflict;
            }


            var client = httpClientFactory.CreateClient("FakeData");
            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId }, jsonOptions),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);

            var httpResponse = await client.PostAsync($"{url}/{ccpId}/order", json);
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
                CCPID= ccpId,
                ValidToDate = softwarePurchaseResponse!.Expiry,
            };

            accountEntry.SoftwareEntries.Add(softwareEntry);
            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }


        public async Task<HttpStatusCode> CancelSubscription(Guid customerId, Guid softwareId)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
            a => a.SoftwareId == softwareId &&
                a.Account.Customer.CustomerId == customerId && a.State!="Cancelled");

            if (softwareEntity == null)
            {
                return HttpStatusCode.NotFound;
            }

            using StringContent json = new(
               JsonSerializer.Serialize(new { accountId =  softwareEntity.Account.AccountId }, jsonOptions),
               Encoding.UTF8,
               MediaTypeNames.Application.Json);

            var client = httpClientFactory.CreateClient("FakeData");
            var httpResponse = await client.PostAsync($"{url}/{softwareEntity.CCPID}/cancel", json);

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
            a => a.SoftwareId == softwareId &&
                a.Account.Customer.CustomerId == customerId && a.State!="Cancelled");

            if (softwareEntity == null)
            {
                return HttpStatusCode.NotFound;
            }

            using StringContent json = new(
              JsonSerializer.Serialize(new { accountId = softwareEntity.Account.AccountId }, jsonOptions),
              Encoding.UTF8,
              MediaTypeNames.Application.Json);

            var client = httpClientFactory.CreateClient("FakeData");
            var httpResponse = await client.PostAsync($"{url}/{softwareEntity.CCPID}/extend", json);

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
