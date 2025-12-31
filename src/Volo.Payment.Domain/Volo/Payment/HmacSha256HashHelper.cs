using System;
using System.Security.Cryptography;
using System.Text;

namespace Volo.Payment;

public static class HmacSha256HashHelper
{
    public static string GetHmacSha256Hash(string hashString, string signature)
    {
        var keyBytes = Encoding.UTF8.GetBytes(signature);
        var messageBytes = Encoding.UTF8.GetBytes(hashString);

        using (HMACSHA256 hash = new HMACSHA256(keyBytes))
        {
            var hashBytes = hash.ComputeHash(messageBytes);

            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
        }
    }
}
