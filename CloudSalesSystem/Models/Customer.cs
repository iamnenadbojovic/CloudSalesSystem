namespace CloudSalesSystem.Models
{
    public class Customer
    {
        public required Guid Id { get; set; }
        public required string Firstname { get; set; }
        public required string Lastname { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required DateTime Created { get; set; }
        public required bool IsActive { get; set; }
        public ICollection<Account> AccountEntries { get; } = [];
    }
}
