namespace CloudSalesSystem.Interfaces
{
    /// <summary>
    /// Login Credentials
    /// </summary>
    public interface ICredentials
    {
        /// <summary>
        /// Password
        /// </summary>
        string? Password { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        string? Username { get; set; }
    }
}