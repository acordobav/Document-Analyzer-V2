using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Claims;

using AuthLibrary.Token;

namespace AuthLibrary.Authorization
{
    class AuthorizationService : IAuthorizationService 
    {
        private readonly ITokenValidator tokenValidator;

        public AuthorizationService(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public bool Authorize(string token)
        {
            return tokenValidator.VerifyToken(token);
        }

        public ClaimsPrincipal Claims 
        {
            get
            {
                return tokenValidator.Claims;
            }
        }
    }
}
