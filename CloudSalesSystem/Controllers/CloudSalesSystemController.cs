using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using CloudSalesSystem.Services.CCPService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudSalesSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudSalesSystemController(
        ILogger<CloudSalesSystemController> logger, 
        ICCPService ccpService, 
        ICustomerService customerService) : ControllerBase
    {

        private readonly ILogger<CloudSalesSystemController> _logger = logger;
        private readonly ICCPService _ccpService = ccpService;
        private readonly ICustomerService _customerService = customerService;

        [HttpGet]
        [Route("services")]
        public async Task<CCPSoftware[]> SoftwareServices()
        {
            var softwareServices = await _ccpService.SoftwareServices();
            return softwareServices;
        }


        [HttpGet]
        [Route("accountList")]
        public async Task<string[]> AccountsList(Guid customerId)
        {
            var accountsEntriesList = await _customerService.CustomerAccounts(customerId);
            var result = accountsEntriesList.Select(x => x.Name).ToArray();
            return result;
        }

        [HttpPost]
        [Route("OrderService")]
        public async Task<HttpResponseMessage> OrderService(Guid accountId, int softwareId)
        {
            var response = await ccpService.OrderSoftware(accountId, softwareId);

            return response;
        }

        [HttpGet]
        [Route("PurchasedSoftware")]
        public async Task<List<Software>> PurchasedSoftware(Guid accountId)
        {
            var response = await customerService.PurchasedSoftware(accountId);

            return response;
        }

        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<bool> UpdateQuantity(Guid softwareId, int quantity)
        {
            var response = await customerService.UpdateLicenceQuantity(softwareId, quantity);

            return response;
        }

        [HttpPut]
        [Route("CancelSubscription")]
        public async Task<bool> CancelAccount(Guid softwareId)
        {
            var response = await customerService.CancelSubscription(softwareId);

            return response;
        }

        [HttpPut]
        [Route("ExtendLicence")]
        public async Task<bool> ExtendLicence(Guid softwareId, int months)
        {
            var response = await customerService.ExtendSoftwareLicence(softwareId, months);

            return response;
        }
    }
}
