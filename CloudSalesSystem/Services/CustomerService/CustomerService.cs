using CloudSalesSystem.DBContext;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.Services.CCPService
{

    public class CustomerService(CloudSalesSystemDbContext cloudSalesSystemDbContext) : ICustomerService
    {
        public async Task<List<Account>> CustomerAccounts(Guid customerId)
        {
            var result = await cloudSalesSystemDbContext.Accounts.Where(a => a.Customer.Id == customerId).ToListAsync();
            return result;
        }

        public async Task<List<Software>> PurchasedSoftware(Guid accountId)
        {
            var result = await cloudSalesSystemDbContext.Softwares.Where(a => a.Account.Id == accountId).ToListAsync();
            return result;
        }

        public async Task<bool> UpdateLicenceQuantity(Guid softwareId, int quantity)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(a => a.Id == softwareId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.Quantity = quantity;

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelSubscription(Guid softwareId)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(a => a.Id == softwareId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.State = "Cancelled";

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExtendSoftwareLicence(Guid softwareId, int days)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.FirstOrDefaultAsync(a => a.Id == softwareId);
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