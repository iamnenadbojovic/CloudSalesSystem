namespace CloudSalesSystem.Models
{
    public class Customer : Credentials
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required DateTime Created { get; set; }
        public ICollection<Account> AccountEntries { get; } = [];
    }
}
