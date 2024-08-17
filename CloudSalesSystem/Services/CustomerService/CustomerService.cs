using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.Services.CCPService
{

    public class CustomerService(CloudSalesSystemDbContext cloudSalesSystemDbContext)
    {
        public Task<List<Account>> CustomerAccounts(Guid customerId)
        {
            var result = cloudSalesSystemDbContext.Accounts.Where(a => a.CustomerEntry.Id == customerId).ToListAsync();
            return result;
        }

        public async Task<List<Software>> PurchasedSoftware(Guid accountId)
        {
            var result = await cloudSalesSystemDbContext.Softwares.Where(a => a.AccountEntry.Id == accountId).ToListAsync();
            return result;
        }

        public async Task<bool> UpdateLicenceQuantity(Guid softwareId, int quantity )
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.SingleAsync(a => a.Id == softwareId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.Quantity = quantity;

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelSoftware(Guid softwareId)
        {
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.SingleAsync(a => a.Id == softwareId);
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
            var softwareEntity = await cloudSalesSystemDbContext.Softwares.SingleAsync(a => a.Id == softwareId);
            if (softwareEntity == null)
            {
                return false;
            }
            softwareEntity.ValidateToDate = softwareEntity.ValidateToDate.AddDays(days);

            await cloudSalesSystemDbContext.SaveChangesAsync();

            return true;
        }
    }

}