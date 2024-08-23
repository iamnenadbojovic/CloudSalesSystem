using CloudSalesSystem.Interfaces;

namespace CloudSalesSystem.Models
{
    public class Credentials : ICredentials
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
