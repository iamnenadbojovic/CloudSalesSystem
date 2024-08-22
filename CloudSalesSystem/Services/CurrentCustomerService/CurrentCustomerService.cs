using CloudSalesSystem.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace CloudSalesSystem.Services.CurrentCustomerService
{
    public class CurrentCustomerService(IHttpContextAccessor httpContextAccessor) : ICurrentCustomerService
    {
        public Guid CustomerId()
        {
            var context = httpContextAccessor.HttpContext;
            string authHeader = context!.Request.Headers["Authorization"]!;

            if (authHeader != null)
            {
                var tokenText = authHeader.Substring(authHeader.IndexOf(" ") + 1).Trim();

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenText);


                return new Guid(token.Claims.First(claim => claim.Type == "nameid").Value);
            }
            return Guid.Empty;
        }
    }
}
