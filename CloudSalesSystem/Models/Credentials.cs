using CloudSalesSystem.Interfaces;

namespace CloudSalesSystem.Models
{
    public class Credentials : ICredentials
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
