using CloudSalesSystem.Models;

namespace CloudSalesSystem.Services.CCPService
{
    public interface ICustomerService
    {
        Task<bool> CancelSubscription(Guid softwareId);
        Task<List<Account>> CustomerAccounts(Guid customerId);
        Task<bool> ExtendSoftwareLicence(Guid softwareId, int days);
        Task<List<Software>> PurchasedSoftware(Guid accountId);
        Task<bool> UpdateLicenceQuantity(Guid softwareId, int quantity);
    }
}