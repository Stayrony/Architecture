using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Architecture.Model.API.Token;
using Architecture.Model.Database.Options;
using Architecture.Model.Shared;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Architecture.Model.Database.Shared
{
    public class TokenService : ITokenService
    {
        private readonly TokenInfo _tokenInfo;

        public TokenService(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _tokenInfo = new TokenInfo
            {
                Audience = jwtTokenOptions.Value.Audience,
                Issuer = jwtTokenOptions.Value.Issuer,
                IssuerSecurityKey = jwtTokenOptions.Value.IssuerSecurityKey,
                Lifetime = jwtTokenOptions.Value.Lifetime,

                ValidateAudience = jwtTokenOptions.Value.ValidateAudience,
                ValidateIssuer = jwtTokenOptions.Value.ValidateIssuer,
                ValidateLifetime = jwtTokenOptions.Value.ValidateLifetime,
                ValidateIssuerSigningKey = jwtTokenOptions.Value.ValidateIssuerSigningKey
            };
        }

        /// <summary>
        /// Gets the token info.
        /// </summary>
        /// <value>The token info.</value>
        public TokenInfo TokenInfo => _tokenInfo;

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="claims">Claims.</param>
        public TokenModel CreateToken(params (object Key, object Value)[] claims)
        {
            return CreateToken(claims.ToLookup(x => x.Key, x => x.Value));
        }

        /// <summary>
        /// Creates the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="claims">Claims.</param>
        public Task<TokenModel> CreateTokenAsync(params (object Key, object Value)[] claims) => Task.Run(() => CreateToken(claims));

        /// <summary>
        /// Creates the token async.
        /// </summary>
        /// <returns>The token async.</returns>
        /// <param name="claimsLookUp">Look up.</param>
        public Task<TokenModel> CreateTokenAsync(ILookup<object, object> claimsLookUp) => Task.Run(() => CreateToken(claimsLookUp));

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="keyValues">Key values.</param>
        public TokenModel CreateToken(IEnumerable<KeyValuePair<object, object>> keyValues) => CreateToken(keyValues.ToLookup(x => x.Key, x => x.Value));

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <returns>The token.</returns>
        /// <param name="claimsLookUp">Claims look up.</param>
        public TokenModel CreateToken(ILookup<object, object> claimsLookUp)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_tokenInfo.Lifetime);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenInfo.Issuer,
                audience: _tokenInfo.Audience,
                notBefore: now,
                claims: claimsLookUp.Any() ? claimsLookUp.Select(x => new Claim(x.Key.ToString(), x.FirstOrDefault()?.ToString())) : null,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenInfo.IssuerSecurityKey)), SecurityAlgorithms.HmacSha256));

            return new TokenModel
            {
                TokenKey = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpireAt = expires
            };
        }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <returns>The claims.</returns>
        /// <param name="token">Handle.</param>
        public IEnumerable<Claim> GetClaims(string token)
        {
            if (!IsTokenValid(token))
                throw new Exception("Handle is invalid");

            var handler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = handler.ReadToken(token.Replace("Bearer ", string.Empty)) as JwtSecurityToken;
            if (jwtSecurityToken == null)
                return Enumerable.Empty<Claim>();

            return jwtSecurityToken.Claims;
        }

        /// <summary>
        /// Ises the token valid.
        /// </summary>
        /// <returns><c>true</c>, if token valid was ised, <c>false</c> otherwise.</returns>
        /// <param name="token">Handle.</param>
        public bool IsTokenValid(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            bool isValid = false;

            SecurityToken securityKey = null;
            try
            {
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuer = _tokenInfo.ValidateIssuer,
                    ValidateLifetime = _tokenInfo.ValidateLifetime,
                    ValidateIssuerSigningKey = _tokenInfo.ValidateIssuerSigningKey,
                    ValidateAudience = _tokenInfo.ValidateAudience,

                    ValidIssuer = _tokenInfo.Issuer,
                    ValidAudience = _tokenInfo.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenInfo.IssuerSecurityKey))

                }, out securityKey);

                isValid = (securityKey != null);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
