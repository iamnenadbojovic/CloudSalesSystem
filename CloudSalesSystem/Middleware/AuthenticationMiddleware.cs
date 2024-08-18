using CloudSalesSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;

namespace CloudSalesSystem.Middleware;
public class AuthenticationMiddleware(RequestDelegate next, CloudSalesSystem.Interfaces.ICredentials credentials)
{

    public async Task Invoke(HttpContext context)
    {
        //Reading the AuthHeader which is signed with JWT
        string authHeader = context.Request.Headers["Authorization"];

        if (authHeader != null)
        {        
            int startPoint = authHeader.IndexOf(".") + 1;

            var tokenText = authHeader.Substring(authHeader.IndexOf("bearer ")+7);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenText);
            context.Items["Name"] = token.Claims.First(claim => claim.Type == "unique_name").Value;
        }
        //Pass to the next middleware
        await next(context);
    }
}