using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystem.DBContext
{
    /// <summary>
    /// Cloud Sales System DbContext
    /// </summary>
    /// <param name="options">DbContextOptions object </param>
    public class CloudSalesSystemDbContext(DbContextOptions<CloudSalesSystemDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// Customers
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Accounts
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Softwares
        /// </summary>
        public virtual DbSet<Software> Softwares { get; set; }
    }
}
