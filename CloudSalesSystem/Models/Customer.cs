using CloudSalesSystem.Interfaces;

namespace CloudSalesSystem.Models
{
    public class Customer
    {
        public ICollection<Account> AccountEntries { get; set; }
    
        public DateTime Created { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
    }
}
