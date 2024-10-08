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

        [HttpPost("login")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CCPSoftware[]>> SoftwareServices()
        {
            var softwareServices = await ccpService.SoftwareServices();
            if (softwareServices == null || softwareServices.Length == 0)
            {
                string message = "Not Found";
                logger.LogError(message);
                return NotFound();
            }
            return Ok(softwareServices);
        }

        [Authorize]
        [HttpGet]
        [Route("accounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [Route("accounts/{accountId}/services/{ccpId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> OrderSoftwareLicence([FromRoute] Guid accountId, [FromRoute] int ccpId)
        {
            var customerId = currentCustomerService.CustomerId();
            var responseStatusCode = await ccpService.OrderSoftwareLicence(customerId, accountId, ccpId);
            return StatusCode((int)responseStatusCode);
        }

        [Authorize]
        [HttpGet]
        [Route("accounts/{accountId}/software")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Software>>> PurchasedSoftware([FromRoute] Guid accountId)
        {
            var customerId = currentCustomerService.CustomerId();
            var response = await customerService.PurchasedSoftware(customerId, accountId);
            if (response == null)
            {
                string message = "Not Found";
                logger.LogError(message);
                return StatusCode(404);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("software/{softwareId}/quantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateQuantity([FromRoute] Guid softwareId, int quantity)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await customerService.UpdateLicenceQuantity(customerId, softwareId, quantity);
            return StatusCode((int)statusCode);
        }

        [Authorize]
        [HttpDelete]
        [Route("software/{softwareId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CancelAccount([FromRoute] Guid softwareId)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await ccpService.CancelSubscription(customerId, softwareId);
            return StatusCode((int)statusCode);
        }

        [Authorize]
        [HttpPut]
        [Route("software/{softwareId}/extend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExtendLicence([FromRoute] Guid softwareId, int months)
        {
            var customerId = currentCustomerService.CustomerId();
            var statusCode = await ccpService.ExtendSoftwareLicence(customerId, softwareId, months);
            return StatusCode((int)statusCode);
        }
    }
}
