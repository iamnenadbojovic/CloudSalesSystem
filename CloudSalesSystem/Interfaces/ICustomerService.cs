using CloudSalesSystem.Models;
using System.Net;

namespace CloudSalesSystem.Services.CCPService
{
    public interface ICustomerService
    {
        Task<List<Account>> CustomerAccounts(Guid customerId);
        Task<List<Software>> PurchasedSoftware(Guid customerId, Guid accountId);
        Task<HttpStatusCode> UpdateLicenceQuantity(Guid customerId, Guid softwareId, int quantity);
    }
}