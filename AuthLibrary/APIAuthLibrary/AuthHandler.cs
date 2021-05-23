using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using AuthLibrary.Configuration;
using AuthLibrary.Factory;

namespace APIAuthLibrary
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IAuthServiceFactory authFactory;

        public AuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthServiceFactory authFactory)
        : base(options, logger, encoder, clock)
        {
            this.authFactory = authFactory;
        }

        /// <summary>
        /// Method that verifies if the request needs to be accepted of not, based on the
        /// authorization provided by IAuthServiceFactory
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            string authorization = Request.Headers["Authorization"];

            // Checks if the Authorization headers exists
            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            // Checks the Authorization type used for the header
            string authType = AuthServiceConfig.Config.AuthType;
            if (authorization.StartsWith(authType, StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring(authType.Length).Trim();
            }

            // Checks if the token is null
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.NoResult();
            }

            // Checks if the token is valid
            bool validToken = authFactory.Authorization.Authorize(token);

            if (!validToken)
            {
                return AuthenticateResult.Fail($"token {authType} not match");
            }
            else
            {
                // The Claims are obtained and issued
                ClaimsPrincipal principal = authFactory.Authorization.Claims;
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }
    }
}
