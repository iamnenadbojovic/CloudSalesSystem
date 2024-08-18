namespace CloudSalesSystem.Models
{
    public class Software
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required int Quantity { get; set; }
        public required string State { get; set; } = "Active";
        public required DateTime ValidateToDate { get; set; }
        public required Account AccountId{ get; set; }
    }
}
