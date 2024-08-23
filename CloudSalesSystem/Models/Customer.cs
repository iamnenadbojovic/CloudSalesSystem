namespace CloudSalesSystem.Models
{
    public class Customer : Credentials
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// AccountEntries
        /// </summary>
        public ICollection<Account> AccountEntries { get; set; } = [];
  }
}
