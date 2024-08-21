using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using CloudSalesSystem.Services.CCPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudSalesSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudSalesSystemController(
        ILogger<CloudSalesSystemController> logger,
        ILoginService loginService,
        ICCPService ccpService,
        ICurrentCustomerService currentCustomerService,
        ICustomerService customerService) : ControllerBase
        {

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var token = await loginService.Login(credentials);
            if (token == null || token == string.Empty)
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        [Route("services")]
        public async Task<CCPSoftware[]> SoftwareServices()
        {           
            var softwareServices = await ccpService.SoftwareServices();
            return softwareServices;
        }

        [Authorize]
        [HttpGet]
        [Route("accountList")]
        public async Task<ActionResult> AccountsList()
        {
            var customerId = currentCustomerService.CustomerId();
            var accountsEntriesList = await customerService.CustomerAccounts(customerId);
     
            return Ok(accountsEntriesList);
        }

        [Authorize]
        [HttpPost]
        [Route("OrderService")]
        public async Task<ActionResult> OrderService(Guid accountId, int softwareId)
        {
            var customerId = currentCustomerService.CustomerId();
            var responseStatusCode = await ccpService.OrderSoftware(customerId, accountId, softwareId);
            return StatusCode((int)responseStatusCode);
        }

        [Authorize]
        [HttpGet]
        [Route("PurchasedSoftware")]
        public async Task<List<Software>> PurchasedSoftware(Guid accountId)
        {
            var customerId = currentCustomerService.CustomerId();
            var response = await customerService.PurchasedSoftware(customerId, accountId);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<bool> UpdateQuantity(Guid softwareId, int quantity)
        {
            var customerId = currentCustomerService.CustomerId();
            var response = await customerService.UpdateLicenceQuantity(customerId, softwareId, quantity);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("CancelSubscription")]
        public async Task<ActionResult> CancelAccount(Guid softwareId)
        {
            var customerId = currentCustomerService.CustomerId();
            var isSucceessfull = await customerService.CancelSubscription(customerId, softwareId);        
            return isSucceessfull ? Ok() : StatusCode(405);
        }

        [Authorize]
        [HttpPut]
        [Route("ExtendLicence")]
        public async Task<ActionResult> ExtendLicence(Guid softwareId, int months)
        {
            var customerId = currentCustomerService.CustomerId();
            var isSuccessful = await customerService.ExtendSoftwareLicence(customerId, softwareId, months);
            return isSuccessful ? Ok() : StatusCode(405);
        }
    }
}
