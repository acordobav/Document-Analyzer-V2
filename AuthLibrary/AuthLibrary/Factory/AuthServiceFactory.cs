using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AuthLibrary.Configuration;
using AuthLibrary.Authorization;
using AuthLibrary.Authorization.Keycloak;

namespace AuthLibrary.Factory
{
    public class AuthServiceFactory : IAuthServiceFactory
    {
        private IAuthorizationService _Authorization = null;

        public AuthServiceFactory() { }

        /// <summary>
        /// Method to obtain an Authorization Service
        /// </summary>
        /// <returns>Authorization Service</returns>
        public IAuthorizationService Authorization
        {
            get
            {
                return _Authorization ??= buildAuthorizationService();
            }
        }

        /// <summary>
        /// Method to build an IAuthorizationService
        /// </summary>
        /// <returns>IAuthorizationService</returns>
        private IAuthorizationService buildAuthorizationService()
        {
            // Getting the keycloak service info
            string keycloakHost = AuthServiceConfig.Config.KeycloakHost;
            string keycloakPort = AuthServiceConfig.Config.KeycloakPort;
            string realmName = AuthServiceConfig.Config.RealmName;

            // Creation of the AuthorizationService
            return new KeycloakClient(keycloakHost, keycloakPort, realmName);
        }
    }
}
