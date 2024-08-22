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
                string message = "Username or Password is incorrect";
                logger.LogError(message);
                return BadRequest(new { message });
            }
            const string success = "Token Acquired";
            logger.LogError(success);
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        [Route("services")]
        public async Task<ActionResult<CCPSoftware[]>> SoftwareServices()
        {
            var softwareServices = await ccpService.SoftwareServices();
            if(softwareServices == null || softwareServices.Length==0)
            {
                string message = "Not Found";
                logger.LogError(message);
                return NotFound();
            }
            return Ok(softwareServices);
        }

        [Authorize]
        [HttpGet]
        [Route("accountList")]
        public async Task<ActionResult> AccountsList()
        {
            var customerId = currentCustomerService.CustomerId();
            var accountsEntriesList = await customerService.CustomerAccounts(customerId);
            if (accountsEntriesList == null)
            {
                string message = "Not Found";
                logger.LogError(message);
                return NotFound();
            }
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
        public async Task<ActionResult<List<Software>>> PurchasedSoftware(Guid accountId)
        {
            var customerId = currentCustomerService.CustomerId();
            var response = await customerService.PurchasedSoftware(customerId, accountId);
            if(response == null)
            {
                string message = "Not Found";
                logger.LogError(message);
                return StatusCode(404);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<ActionResult> UpdateQuantity(Guid softwareId, int quantity)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await customerService.UpdateLicenceQuantity(customerId, softwareId, quantity);
            return StatusCode((int)statusCode);
        }

        [Authorize]
        [HttpPut]
        [Route("CancelSubscription")]
        public async Task<ActionResult> CancelAccount(Guid softwareId)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await ccpService.CancelSubscription(customerId, softwareId);
            return StatusCode((int)statusCode);
        }

        [Authorize]
        [HttpPut]
        [Route("ExtendLicence")]
        public async Task<ActionResult> ExtendLicence(Guid softwareId, int months)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await ccpService.ExtendSoftwareLicence(customerId, softwareId, months);
            return StatusCode((int)statusCode);
        }
    }
}
