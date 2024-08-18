using CloudSalesSystem.Models;

namespace CloudSalesSystem.Interfaces
{
    public interface ILoginService
    {
        Task<string> Login(Credentials credentials);
    }
}
