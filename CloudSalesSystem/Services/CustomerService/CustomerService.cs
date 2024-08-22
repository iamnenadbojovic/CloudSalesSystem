using CloudSalesSystem.DBContext;
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

        public async Task<List<Software>> PurchasedSoftware(Guid customerId, Guid accountId)
        {
            var result = await cloudSalesSystemDbContext.Softwares.Where(
                a => a.Account.Id == accountId &&
                a.Account.Customer.Id == customerId).ToListAsync();
            return result;
        }

       
    }
}