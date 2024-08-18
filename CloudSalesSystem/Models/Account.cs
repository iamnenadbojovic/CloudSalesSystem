namespace CloudSalesSystem.Models
{
    public class Account
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string CCPAccountId { get; set; }
        public required DateTime Created { get; set; }
        public required Customer Customer { get; set; }
        public ICollection<Software> SoftwareEntries { get; } = [];
    }
}
