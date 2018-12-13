using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSocialNetwork.Models.Services
{
    public static class AuthenticationService
    {
        public static string EncryptPassword(string password)
        {
            using (var sha512 = new SHA512Managed())
            {
                var utf8 = new UTF8Encoding();
                var data = sha512.ComputeHash(utf8.GetBytes(password));
                return Convert.ToBase64String(data).Substring(0, 30);
            }
        }
    }
}
