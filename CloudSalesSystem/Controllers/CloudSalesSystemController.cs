using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using CloudSalesSystem.Services.CCPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CloudSalesSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudSalesSystemController(
        ILogger<CloudSalesSystemController> logger,
        ILoginService loginService,
        ICCPService ccpService, 
        ICustomerService customerService) : ControllerBase
    {

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var token = await loginService.Login(credentials);
            if (token == null || token == string.Empty)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
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
        public async Task<string[]> AccountsList(Guid customerId)
        {
            var accountsEntriesList = await customerService.CustomerAccounts(customerId);
            var result = accountsEntriesList.Select(x => x.Name).ToArray();
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("OrderService")]
        public async Task<HttpResponseMessage> OrderService(Guid accountId, int softwareId)
        {
            var name = HttpContext.Items["Name"];
            var response = await ccpService.OrderSoftware(accountId, softwareId);
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("PurchasedSoftware")]
        public async Task<List<Software>> PurchasedSoftware(Guid accountId)
        {
            var name = HttpContext.Items["Name"];
            var response = await customerService.PurchasedSoftware(accountId);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateQuantity")]
        public async Task<bool> UpdateQuantity(Guid softwareId, int quantity)
        {
            var name = HttpContext.Items["Name"];
            var response = await customerService.UpdateLicenceQuantity(softwareId, quantity);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("CancelSubscription")]
        public async Task<bool> CancelAccount(Guid softwareId)
        {
            var name = HttpContext.Items["Name"];
            var response = await customerService.CancelSubscription(softwareId);
            return response;
        }

        [Authorize]
        [HttpPut]
        [Route("ExtendLicence")]
        public async Task<bool> ExtendLicence(Guid softwareId, int months)
        {
            var name = HttpContext.Items["Name"];
            var response = await customerService.ExtendSoftwareLicence(softwareId, months);
            return response;
        }
    }
}
