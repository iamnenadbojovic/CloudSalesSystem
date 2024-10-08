﻿using CloudSalesSystem.DBContext;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace CloudSalesSystemTests
{
    public class BaseTests
    {
        protected   readonly DbContextOptions<CloudSalesSystemDbContext> options =
          new DbContextOptionsBuilder<CloudSalesSystemDbContext>().Options;

        protected Mock<CloudSalesSystemDbContext> mockContext;

        protected static readonly Customer customer = new()
        {
            CustomerId = new Guid("C4B63093-52F4-4EC3-B6B3-FDBE9C2B76D3"),
            Username = "nenad",
            Password = "test",
            Created = DateTime.Now
        };

        protected static List<Customer> customers = [customer];

        protected static readonly Account account = new()
        {
            AccountId = new Guid("7D6ABF8E-6A2E-4606-9714-1175B7B4DE73"),
            Name = "Office Account",
            Customer = customer,
            CCPAccountId = "AAB63093-52F4-4EC3-B6B3-FDBE9C2B76D3"
        };

        protected static List<Account> accounts = [account];

        protected static readonly Software softwareEntry = new()
        {
            SoftwareId = new Guid("2D6ABF8E-6A2E-4606-9714-1175B7B4DE73"),
            Name = "Office Account",
            Account = account,
            CCPID = 2,
            ValidToDate = DateTime.Now,
            Quantity = 1,
            State = "Active"
        };

        protected static List<Software> softwares = [softwareEntry];


        public BaseTests()
        {
            mockContext = new Mock<CloudSalesSystemDbContext>(options);

            mockContext.Setup(m => m.Customers).ReturnsDbSet(customers);
            mockContext.Setup(m => m.Accounts).ReturnsDbSet(accounts);
            mockContext.Setup(m => m.Softwares).ReturnsDbSet(softwares);
        }
    }
}