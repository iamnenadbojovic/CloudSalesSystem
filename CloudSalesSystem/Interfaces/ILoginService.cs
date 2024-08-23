using CloudSalesSystem.Models;

namespace CloudSalesSystem.Interfaces
{
    /// <summary>
    /// Login Service
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Provides login customerId
        /// </summary>
        /// <param name="credentials">Credentials Object</param>
        /// <returns></returns>
        Task<string> Login(Credentials credentials);
    }
}
