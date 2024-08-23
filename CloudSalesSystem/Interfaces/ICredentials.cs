namespace CloudSalesSystem.Interfaces
{
    public interface ICredentials
    {
        string? Password { get; set; }
        string? Username { get; set; }
    }
}