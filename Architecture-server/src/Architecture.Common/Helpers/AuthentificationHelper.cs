using System;
using System.Linq;
using System.Security.Cryptography;

namespace Architecture.Common.Helpers
{
    public static class AuthentificationHelper
    {
        public static (byte[] hash, byte[] salt) HashPasswordAndSalt(string password)
        {
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 10000);
            return (rfc2898DeriveBytes.GetBytes(20), rfc2898DeriveBytes.Salt);
        }

        public static bool Verify(string password, byte[] salt, byte[] hash)
        {
            if (password == null
             || salt == null
             || hash == null)
                return false;

            return new Rfc2898DeriveBytes(password, salt, 10000)
                   .GetBytes(20)
                   .Zip(hash, (b, c) => (b, c))
                   .All(x => x.Item1 == x.Item2);
        }
    }
}
