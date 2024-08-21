using CloudSalesSystem.Models;
using System.IdentityModel.Tokens.Jwt;

namespace CloudSalesSystem.Interfaces
{
    public interface ICurrentCustomerService
    {
        public Guid CustomerId();
     
    }
}