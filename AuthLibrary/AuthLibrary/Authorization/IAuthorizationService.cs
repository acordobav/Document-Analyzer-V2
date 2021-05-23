using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Claims;

namespace AuthLibrary.Authorization
{
    public interface IAuthorizationService
    {
        public bool Authorize(string token);

        public ClaimsPrincipal Claims { get; }
    }
}
