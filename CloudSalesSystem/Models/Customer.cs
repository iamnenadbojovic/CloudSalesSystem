namespace CloudSalesSystem.Models
{
    public class Customer : Credentials
    {
        public Guid CustomerId { get; set; }
        public DateTime Created { get; set; }
        public string? Email { get; set; }
        public ICollection<Account> AccountEntries { get; set; } = [];
  }
}
