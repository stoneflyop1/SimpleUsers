using System;
using Microsoft.IdentityModel.Tokens;

namespace SimpleUsers.WebAPI.Providers
{
    #pragma warning disable 1591
    
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/api/User/Login";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(500);
        public SigningCredentials SigningCredentials { get; set; }
    }
}