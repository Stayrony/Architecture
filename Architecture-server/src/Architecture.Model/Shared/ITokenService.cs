using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Architecture.Model.API.Token;

namespace Architecture.Model.Shared
{
    public interface ITokenService
    {
        TokenInfo TokenInfo { get; }
        TokenModel CreateToken(params (object Key, object Value)[] claims);
        TokenModel CreateToken(ILookup<object, object> lookUp);
        TokenModel CreateToken(IEnumerable<KeyValuePair<object, object>> keyValues);
        Task<TokenModel> CreateTokenAsync(ILookup<object, object> lookUp);
        Task<TokenModel> CreateTokenAsync(params (object Key, object Value)[] claims);
        bool IsTokenValid(string token);
        IEnumerable<Claim> GetClaims(string token);
    }

    public class TokenInfo
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }

        public int Lifetime { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string IssuerSecurityKey { get; set; }
    }
}
