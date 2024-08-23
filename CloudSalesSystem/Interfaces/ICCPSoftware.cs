namespace CloudSalesSystem.Interfaces
{
    /// <summary>
    /// ICCP Software Response object
    /// </summary>
    public interface ICCPSoftware
    {
        /// <summary>
        /// Software Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Software Name
        /// </summary>
        string? Name { get; set; }
    }
}
