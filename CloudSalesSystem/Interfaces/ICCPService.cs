using CloudSalesSystem.Models;
using System.Net;

namespace CloudSalesSystem.Interfaces
{
    public interface ICCPService
    {
        Task<CCPSoftware[]> SoftwareServices();
        Task<HttpStatusCode> OrderSoftware(Guid customerId, Guid accountId, int softwareId);
        Task<HttpStatusCode> CancelSubscription(Guid customerId, Guid softwareId);
        Task<HttpStatusCode> ExtendSoftwareLicence(Guid customerId, Guid softwareId, int days);
    }
}
