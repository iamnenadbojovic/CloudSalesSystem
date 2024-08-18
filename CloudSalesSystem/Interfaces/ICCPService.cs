using CloudSalesSystem.Models;

namespace CloudSalesSystem.Interfaces
{
    public interface ICCPService
    {
        Task<string[]> SoftwareServices();
        Task<HttpResponseMessage> OrderSoftware(Guid accountId, Software softwareService);
    }
}
