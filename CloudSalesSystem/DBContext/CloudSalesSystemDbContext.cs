using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.DBContext
{
    public class CloudSalesSystemDbContext(DbContextOptions<CloudSalesSystemDbContext> options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Software> Softwares { get; set; }

    }
}
