using CloudSalesSystem.Models;
using System.Net;

namespace CloudSalesSystem.Interfaces
{
    /// <summary>
    /// ICCPService
    /// </summary>
    public interface ICCPService
    {
        /// <summary>
        /// Software service s array
        /// </summary>
        /// <returns></returns>
        Task<CCPSoftware[]> SoftwareServices();

        /// <summary>
        /// OrderSoftwareLicence
        /// </summary>
        /// <param name="customerId">Guid CustomerId</param>
        /// <param name="accountId">Guid AccountId</param>
        /// <param name="ccpId">int ccpId</param>
        /// <returns></returns>
        Task<HttpStatusCode> OrderSoftwareLicence(Guid customerId, Guid accountId, int ccpId);

        /// <summary>
        /// Cancels Subscription
        /// </summary>
        /// <param name="customerId">Guid CustomerId</param>
        /// <param name="accountId">Guid AccountId</param>
        /// <returns></returns>
        Task<HttpStatusCode> CancelSubscription(Guid customerId, Guid softwareId);

        /// <summary>
        /// Extends Software Licence
        /// </summary>
        /// <param name="customerId">Guid CustomerId</param>
        /// <param name="softwareId">Guid SoftwareId</param>
        /// <param name="days">int number of days</param>
        /// <returns></returns>
        Task<HttpStatusCode> ExtendSoftwareLicence(Guid customerId, Guid softwareId, int days);
    }
}
