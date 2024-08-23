namespace CloudSalesSystem.Interfaces
{
    /// <summary>
    /// Current Customer Service
    /// </summary>
    public interface ICurrentCustomerService
    {
        /// <summary>
        /// Customer Id
        /// </summary>
        /// <returns>Guid</returns>
        public Guid CustomerId();
    }
}