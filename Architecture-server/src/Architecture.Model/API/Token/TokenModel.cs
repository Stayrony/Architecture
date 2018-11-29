using System;
namespace Architecture.Model.API.Token
{
    public class TokenModel
    {
        public string TokenKey { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
