using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SimpleUsers.WebAPI.Providers
{
    #pragma warning disable 1591
    
    public class TokenUtil
    {
        public static readonly string SecretKey = "mysupersecret_secretkey!123";

        // https://stackoverflow.com/questions/37708266/bearer-token-authentication-in-asp-net-core
        //https://github.com/aspnet/Security/issues/1310
        public static TokenValidationParameters Create()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            return new TokenValidationParameters
            {
                // The signing key must match! 
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim 
                ValidateIssuer = true,
                ValidIssuer = "SimpleUsers",
                // Validate the JWT Audience (aud) claim 
                ValidateAudience = true,
                ValidAudience = "SimpleUsersAudience",
                // Validate the token expiry 
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here: 
                ClockSkew = TimeSpan.Zero
            };
        }

        public static TokenProvider GetProvider()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var options = new TokenProviderOptions
            {
                Audience = "SimpleUsersAudience",
                Issuer = "SimpleUsers",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
            return new TokenProvider(options);
        }
    }
}