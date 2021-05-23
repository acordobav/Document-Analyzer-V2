using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthLibrary.Authorization;


namespace AuthLibrary.Factory
{
    public interface IAuthServiceFactory
    {
        public IAuthorizationService Authorization { get; }
    }
}
