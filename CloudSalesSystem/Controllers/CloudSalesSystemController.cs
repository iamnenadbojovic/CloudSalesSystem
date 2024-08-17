using Microsoft.AspNetCore.Mvc;

namespace CloudSalesSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudSalesSystemController : ControllerBase
    {


        private readonly ILogger<CloudSalesSystemController> _logger;

        public CloudSalesSystemController(ILogger<CloudSalesSystemController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult Get()
        {
            return null;
        }
    }
}
