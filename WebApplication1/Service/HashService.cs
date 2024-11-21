using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace WebApplication1.Service
{
    public static class HashService
    {
        public static string HashString(string passwordString)
        {
            string mySalt = "@!#@!!124615162341@#!@216!#@!!124615162341@#!@21635162345#!#@!!124615162341@#!@21635162345#35162345#";
            var salt = System.Text.Encoding.UTF8.GetBytes(mySalt);
            var password = System.Text.Encoding.UTF8.GetBytes(passwordString);
            var hmacMD5 = new HMACMD5(salt);
            var saltedHash = hmacMD5.ComputeHash(password);
            var hmacSHA1 = new HMACSHA1(salt);
            return Convert.ToBase64String(hmacSHA1.ComputeHash(saltedHash));
            
        }
    }
}
