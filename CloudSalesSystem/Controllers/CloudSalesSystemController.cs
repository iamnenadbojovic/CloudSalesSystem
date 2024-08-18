using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<string[]> SoftwareServices()
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
    }
}
