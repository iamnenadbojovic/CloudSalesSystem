using CloudSalesSystem.Models;

namespace CloudSalesSystem.DBContext
{
    public static class SeedDatabase
    {
        public static void Initialize(CloudSalesSystemDbContext context)
        {
            context.Database.EnsureCreated();


            var firstCustomer = new Customer
            {
                CustomerId = new Guid("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3"),
                Username = "Nenad",
                Password = "12345",
                Created = DateTime.Now,
                Email = "nenad.bojovic@gmail.com"
            };
            var secondCustomer = new Customer
            {
                CustomerId = new Guid("A4B63093-52F4-4EC3-A6A3-FDBE9C2B76D3"),
                Username = "Ashley",
                Password = "54321",
                Created = DateTime.Now,
                Email = "ashley.addams@gmail.com"
            };
            var firstAccount = new Account
            {
                AccountId = new Guid("C4B63093-12F4-4EC3-B6B3-FDBE9C2B76D3"),
                CCPAccountId = "389215ba-3a17-4c09-884f-a2afd8eda1d9",
                Name = "Microsoft Azure",
                Created = DateTime.Now,
                Customer = firstCustomer,
                CustomerId = firstCustomer.CustomerId,

            };
            var secondAccount = new Account
            {
                AccountId = new Guid("C4B63093-22F4-4EC3-B6B3-FDBE9C2B76D3"),
                CCPAccountId = "229215ba-3a17-4c09-884f-a2afd8eda1d9",
                Name = "Ashley Software",
                Customer = secondCustomer,
                CustomerId = secondCustomer.CustomerId,
            };
            var seeded = context.Customers.Any(a => a.CustomerId == firstCustomer.CustomerId);
            if (!seeded)
            {
                firstCustomer.AccountEntries.Add(firstAccount);
                secondCustomer.AccountEntries.Add(secondAccount);
                context.AddRange(firstCustomer,secondCustomer);
                context.SaveChanges();
            }

        }
    }
}
