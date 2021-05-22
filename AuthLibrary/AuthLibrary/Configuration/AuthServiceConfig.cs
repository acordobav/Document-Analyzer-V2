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

        private string _secretKey;
        private string _issuerToken;
        private int _expirationTime;
        private string _authType;

        /// <summary>
        /// SecretKey used to encrypt the token
        /// </summary>
        public string SecretKey
        {
            get { return _secretKey; }
            set { _secretKey = value; }
        }

        /// <summary>
        /// Signurate of the issuer application
        /// </summary>
        public string IssuerToken
        {
            get { return _issuerToken; }
            set { _issuerToken = value; }
        }

        /// <summary>
        /// Expiration time of a new token
        /// </summary>
        public int ExpirationTime
        {
            get { return _expirationTime; }
            set { _expirationTime = value; }
        }

        /// <summary>
        /// Authentication type
        /// </summary>
        public string AuthType
        {
            get { return _authType; }
            set { _authType = value; }
        }
    }
}
