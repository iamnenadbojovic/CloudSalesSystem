using CloudSalesSystem.Interfaces;

namespace CloudSalesSystem.Models
{
    public class CCPSoftware : ICCPSoftware
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; set; }
    }
}
