using CloudSalesSystem.Models;

namespace CloudSalesSystem.Interfaces
{
    public interface ICCPService
    {
        Task<CCPSoftware[]> SoftwareServices();
        Task<HttpResponseMessage> OrderSoftware(Guid accountId, int softwareId);
    }
}
