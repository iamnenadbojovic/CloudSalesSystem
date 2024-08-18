using System.Text.Json.Serialization;

namespace CloudSalesSystem.Models
{
    public class Account
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public required string CCPAccountId { get; set; }
        public required DateTime Created { get; set; }
        [JsonIgnore]
        public required Customer Customer { get; set; }

        [JsonIgnore]
        public ICollection<Software> SoftwareEntries { get; set; } = [];
    }
}
