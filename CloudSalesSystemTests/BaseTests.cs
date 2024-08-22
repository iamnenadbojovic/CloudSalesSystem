using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudSalesSystemTests
{
    public class BaseTests
    {
        protected  readonly DbContextOptions<CloudSalesSystemDbContext> options =
          new DbContextOptionsBuilder<CloudSalesSystemDbContext>().Options;

        protected static readonly Customer customer = new()
        {
            Id = new Guid("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3"),
            Username = "nenad",
            Password = "test",
            Created = DateTime.Now
        };

        protected static readonly Account account = new()
        {
            Id = new Guid("7D6ABF8E-6A2E-4606-9714-1175B7B4DE73"),
            Name = "Office Account",
            Customer = customer,
            CCPAccountId = "AAB63093-52F4-4EC3-B6B3-FDBE9C2B76D3"
        };

        protected static readonly Software softwareEntry = new()
        {
            Id = new Guid("2D6ABF8E-6A2E-4606-9714-1175B7B4DE73"),
            Name = "Office Account",
            Account = account,
            CCPID = 2,
            ValidToDate = DateTime.Now,
            Quantity = 1,
            State = "Active"
        };

    }
}