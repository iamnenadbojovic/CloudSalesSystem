using CloudSalesSystem.DBContext;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudSalesSystem.Services.LoginService
{
    public class LoginService(CloudSalesSystemDbContext cloudSalesSystemDbContext, IConfiguration configuration) : ILoginService
    {
     
        public async Task<string> Login(Credentials credentials)
        {
            var loginCustomer = await cloudSalesSystemDbContext.Customers.FirstOrDefaultAsync(
                x => x.Username == credentials.Username && x.Password == credentials.Password);

            if (loginCustomer == null)
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = configuration["Jwt:Key"];

            if(jwtKey == null) {
                return string.Empty;
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, credentials.Username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    key: new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string customerToken = tokenHandler.WriteToken(token);
            return customerToken;
        }
    }
}
