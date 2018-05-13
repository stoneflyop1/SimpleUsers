using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimpleUsers.Core.Models;

namespace SimpleUsers.WebAPI.Providers
{
    #pragma warning disable 1591
    
    public class TokenProvider
    {
        private readonly TokenProviderOptions _options;
        public TokenProvider(TokenProviderOptions options)
        {
            _options = options;
        }

        public TokenEntity GenerateToken(UserDto user)
        {
            var identity = GetIdentity(user);
            if (identity == null)
                return null;

            DateTime now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Prn, user.UserId),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenEntity
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            return response;
        }
        private ClaimsIdentity GetIdentity(UserDto user)
        {
            return new ClaimsIdentity(
                new System.Security.Principal.GenericIdentity(user.UserName, "Token"),
                new [] { new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId) });
        }
        public static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }

    public class TokenEntity
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
}