using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthLibrary.Configuration
{
    /// <summary>
    /// The purporse of this class is to provide a place to put the AuthLibrary configuration, 
    /// like the secret key used to encrypt the token, or the expiration time of the token
    /// </summary>
    public class AuthServiceConfig
    {
        // The constructor is private because this class implements the Singleton Pattern
        private AuthServiceConfig() { }

        private static readonly Lazy<AuthServiceConfig> lazy = new Lazy<AuthServiceConfig>(() => new AuthServiceConfig());

        public static AuthServiceConfig Config
        {
            get
            {
                return lazy.Value;
            }
        }

        private string _authType = null;
        private string _keycloakHost = null;
        private string _keycloakPort = null;
        private string _realmName = null;

        /// <summary>
        /// Authentication type
        /// </summary>
        public string AuthType
        {
            get { return _authType; }
            set { _authType = value; }
        }

        /// <summary>
        /// IP of the keycloak server
        /// </summary>
        public string KeycloakHost
        {
            get { return _keycloakHost; }
            set { _keycloakHost = value; }
        }

        /// <summary>
        /// Port used by the keycloak server
        /// </summary>
        public string KeycloakPort
        {
            get { return _keycloakPort; }
            set { _keycloakPort = value; }
        }

        /// <summary>
        /// Name of the application's realm
        /// </summary>
        public string RealmName
        {
            get { return _realmName; }
            set { _realmName = value; }
        }
    }
}
