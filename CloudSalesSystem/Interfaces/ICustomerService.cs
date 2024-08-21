using CloudSalesSystem.Models;

namespace CloudSalesSystem.Services.CCPService
{
    public interface ICustomerService
    {
        Task<bool> CancelSubscription(Guid customerId, Guid softwareId);
        Task<List<Account>> CustomerAccounts(Guid customerId);
        Task<bool> ExtendSoftwareLicence(Guid customerId, Guid softwareId, int days);
        Task<List<Software>> PurchasedSoftware(Guid customerId, Guid accountId);
        Task<bool> UpdateLicenceQuantity(Guid customerId, Guid softwareId, int quantity);
    }
}