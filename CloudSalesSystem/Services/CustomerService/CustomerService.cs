using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CloudSalesSystem.Services.CCPService
{

    public class CustomerService(CloudSalesSystemDbContext cloudSalesSystemDbContext) : ICustomerService
    {
        public async Task<List<Account>> CustomerAccounts(Guid customerId)
        {
            var result = await cloudSalesSystemDbContext.Accounts.Where(a => a.Customer.Id == customerId).ToListAsync();
            return result;
        }

        public async Task<List<Software>> PurchasedSoftware(Guid customerId, Guid accountId)
        {
            var result = await cloudSalesSystemDbContext.Softwares.Where(
                a => a.Account.Id == accountId &&
                a.Account.Customer.Id == customerId).ToListAsync();
            return result;
        }

        public async Task<HttpStatusCode> UpdateLicenceQuantity(Guid customerId, Guid softwareId, int quantity)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(
                a => a.Id == softwareId &&
                a.Account.Customer.Id == customerId && a.State != "Cancelled");

            if (softwareEntity == null)
            {
                return HttpStatusCode.NotFound;
            }
            softwareEntity.Quantity = quantity;

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}