using System;
namespace Architecture.Model.Database.Options
{
    public class JwtTokenOptions
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
