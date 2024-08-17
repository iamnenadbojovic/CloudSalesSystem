using Microsoft.Extensions.Hosting;

namespace CloudSalesSystem.Models
{
    public class Account
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required DateTime Created { get; set; }
        public required Customer CustomerEntry { get; set; }
        public ICollection<Software> SoftwareEntries { get; } = [];
    }
}
