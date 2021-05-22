using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AuthLibrary.Token
{
    public interface ITokenGenerator
    {
        public string GenerateToken(IEnumerable<KeyValuePair<string, string>> claims);
    }
}
