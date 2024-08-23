using System.Text.Json.Serialization;

namespace CloudSalesSystem.Models
{
    public class Account
    {
        /// <summary>
        /// Account Id
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// CCPAccountId
        /// </summary>
        [JsonIgnore]
        public string? CCPAccountId { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Customer
        /// </summary>
        [JsonIgnore]
        public Customer Customer { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [JsonIgnore]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// SoftwareEntries
        /// </summary>
        [JsonIgnore]
        public ICollection<Software> SoftwareEntries { get; set; } = [];
    }
}
