namespace CloudSalesSystem.Models
{
    public class Customer : Credentials
    {
        public ICollection<Account> AccountEntries { get; set; } = [];

        public DateTime Created { get; set; }
        public string? Email { get; set; }
        public Guid Id { get; set; }
    }
}
