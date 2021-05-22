using System;

using AuthLibrary.Configuration;
using AuthLibrary.Factory;

using System.Collections.Generic;
using System.Security.Claims;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            AuthServiceConfig.Config.SecretKey = "shfvoilf4ltf645sf4%";
            AuthServiceConfig.Config.IssuerToken = "Test Issuer";
            AuthServiceConfig.Config.ExpirationTime = 1;

            IAuthServiceFactory authFactory = new AuthServiceFactory();

            string email = "test@email.company.com";
            string id = "4";

            var tokenClaims = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>(ClaimTypes.Email, email),
                new KeyValuePair<string, string>(ClaimTypes.NameIdentifier, id)
            };

            string token = authFactory.TokenGenerator.GenerateToken(tokenClaims);

            Console.WriteLine(token);

            Console.WriteLine("");

            authFactory.Authorization.Authorize(token);

            ClaimsPrincipal claims = authFactory.Authorization.Claims;

            Console.WriteLine(claims.FindFirst(ClaimTypes.Email).Value);
        }
    }
}
