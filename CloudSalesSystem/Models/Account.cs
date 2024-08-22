using System.Text.Json.Serialization;

namespace CloudSalesSystem.Models
{
    public class Account
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public string? CCPAccountId { get; set; }

        public DateTime Created { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        [JsonIgnore]
        public ICollection<Software> SoftwareEntries { get; set; } = [];
    }
}
