using System.Text.Json.Serialization;

namespace CloudSalesSystem.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public string? CCPAccountId { get; set; }

        public DateTime Created { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        [JsonIgnore]
        public Guid CustomerId { get; set; }

        [JsonIgnore]
        public ICollection<Software> SoftwareEntries { get; set; } = [];
    }
}
