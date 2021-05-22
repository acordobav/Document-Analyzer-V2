using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Claims;

namespace AuthLibrary.Token
{
    public interface ITokenValidator
    {
        public ClaimsPrincipal Claims { get; }

        public bool VerifyToken(string token);
    }
}
