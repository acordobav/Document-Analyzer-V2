using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Reflection;


namespace AuthLibrary.Authorization.Keycloak
{
    public class KeycloakClient : IAuthorizationService
    {
        private string _baseAddress;
        private string _authUrl;
        private ClaimsPrincipal _claims;

        public KeycloakClient(string keycloakHost, string keycloakPort, string realmName)
        {
            if (keycloakPort != null)
            {
                // Creating the service endpoint url
                _baseAddress = "http://" + keycloakHost + ":" + keycloakPort + "/";
            } 
            else
            {
                // Creating the service endpoint url
                _baseAddress = "http://" + keycloakHost + "/";
            }

            // Creating the realm authorization endpoint
            _authUrl = "auth/realms/" + realmName + "/protocol/openid-connect/userinfo";
        }

        public ClaimsPrincipal Claims
        {
            get
            {
                return _claims;
            }
        }

        /// <summary>
        /// This method verify an encrypted token
        /// </summary>
        /// <param name="token">Encrypted token</param>
        /// <returns>true if the token is valid, false otherwise</returns>
        public bool Authorize(string token)
        {
            var task = KeycloakAuthorize(token);
            return task.GetAwaiter().GetResult();
        }

        private async Task<bool> KeycloakAuthorize(string token)
        {
            using (var client = new HttpClient())
            {
                // Passing service url
                client.BaseAddress = new Uri(_baseAddress);

                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Sending request to verify the valid status of the token
                HttpResponseMessage response = await client.GetAsync(_authUrl);
                if(response.IsSuccessStatusCode)
                {
                    // Gets the response content
                    string raw_content = response.Content.ReadAsStringAsync().Result;
                    
                    // Builds claims
                    build_claims(raw_content);

                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        private void build_claims(string response_content)
        {
            KeycloakResponse content = JsonSerializer.Deserialize<KeycloakResponse>(response_content);

            // Creates the claim list with the response content
            List<Claim> claims = new List<Claim>();
            PropertyInfo[] properties = typeof(KeycloakResponse).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name; // Gets the name of the property
                string value = property.GetValue(content).ToString(); // Gets the value of the property
                Claim claim = new Claim(name, value); // Creates the claim
                claims.Add(claim); // Adds the claim
            }

            // Creates the ClaimsIdentity object and ClaimsPrincipal object
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(claimsIdentity);

            // Stores the created claims
            _claims = claimsPrincipal;
        }
    }
}
