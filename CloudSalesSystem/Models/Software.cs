using System.Text.Json.Serialization;

namespace CloudSalesSystem.Models
{
    public class Software
    {
        /// <summary>
        /// SoftwareId
        /// </summary>
        public Guid SoftwareId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// CCPID
        /// </summary>
        public required int CCPID { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; } = "Active";

        /// <summary>
        /// ValidToDate
        /// </summary>
        public required DateTime ValidToDate { get; set; }

        /// <summary>
        /// AccountId
        /// </summary>
        [JsonIgnore]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Account
        /// </summary>
        [JsonIgnore]
        public Account Account { get; set; } = new Account();
    }
}
