using CloudSalesSystem.Interfaces;

namespace CloudSalesSystem.Models
{
    public class CCPSoftware : ICCPSoftware
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
