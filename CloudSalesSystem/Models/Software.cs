﻿namespace CloudSalesSystem.Models
{
    public class Software
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public required int Quantity { get; set; }
        public string State { get; set; } = "Active";
        public required DateTime ValidToDate { get; set; }
        public Account Account { get; set; } = new Account();
    }
}
