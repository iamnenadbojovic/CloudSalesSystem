using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.DBContext
{
    public class CloudSalesSystemDbContext(DbContextOptions<CloudSalesSystemDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual  DbSet<Account> Accounts { get; set; }
        public virtual  DbSet<Software> Softwares { get; set; }

    }
}
