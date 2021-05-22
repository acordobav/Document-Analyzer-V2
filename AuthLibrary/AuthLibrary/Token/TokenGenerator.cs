using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace AuthLibrary.Token
{
    class TokenGenerator : ITokenGenerator
    {
        private string secretKey;
        private int expirationTime; // Minutes untilt the token is invalid
        private string issuerToken;

        public TokenGenerator(string secretKey, string issuerToken, int expirationTime)
        {
            this.secretKey = secretKey;
            this.issuerToken = issuerToken;
            this.expirationTime = expirationTime;
        }

        /// <summary>
        /// This method implements the JWT Token Generators
        /// </summary>
        /// <param name="claims"> List of info that need to be stored in the JWT token
        /// First element: email
        /// Second element: id
        /// </param>
        /// <returns>JWT Token</returns>        
        public string GenerateToken(IEnumerable<KeyValuePair<string, string>> claims)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Create a Claims Identity
            ClaimsIdentity claimsIdentity = GenerateClaimsIdentity(claims);

            // Create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirationTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }

        /// <summary>
        /// Method to create a ClaimsIdentity object that contains all the individual claims received as argument
        /// </summary>
        /// <param name="claims">KeyValuePair with the Claim type and the Claim value</param>
        /// <returns>ClaimsIdentity generated</returns>
        private ClaimsIdentity GenerateClaimsIdentity(IEnumerable<KeyValuePair<string, string>> claims)
        {
            // Checks if stringClaims is null
            if (claims == null) return null;

            // Creation of Claims list
            List<Claim> claimList = new List<Claim>();

            // Creation of each individual claim
            foreach(KeyValuePair<string, string> claim in claims)
            {
                Claim newClaim = new Claim(claim.Key, claim.Value);
                claimList.Add(newClaim);
            }
            // Create a Claims Identity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimList);

            return claimsIdentity;
        }
    }
}
