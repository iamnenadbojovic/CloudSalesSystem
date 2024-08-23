using CloudSalesSystem.Models;
using System.Net;

namespace CloudSalesSystem.Services.CCPService
{
    /// <summary>
    /// Customer Service
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Retrieves Customer Accounts
        /// </summary>
        /// <param name="customerId">Guid Customer Id</param>
        /// <returns>List of Accounts</returns>
        Task<List<Account>> CustomerAccounts(Guid customerId);

        /// <summary>
        /// Retrieves Purchased Software
        /// </summary>
        /// <param name="customerId">Guid Customer Id</param>
        /// <param name="accountId">Guid Account Id</param>
        /// <returns>List<Software></returns>
        Task<List<Software>> PurchasedSoftware(Guid customerId, Guid accountId);

        /// <summary>
        /// Updates Licence Quantity
        /// </summary>
        /// <param name="customerId">Guid Customer Id</param>
        /// <param name="softwareId">Guid softwareId</param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        Task<HttpStatusCode> UpdateLicenceQuantity(Guid customerId, Guid softwareId, int quantity);
    }
}