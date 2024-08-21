using CloudSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudSalesSystem.Interfaces
{
    public interface ICCPService
    {
        Task<CCPSoftware[]> SoftwareServices();
        Task<HttpStatusCode> OrderSoftware(Guid customerId, Guid accountId, int softwareId);
    }   
}
