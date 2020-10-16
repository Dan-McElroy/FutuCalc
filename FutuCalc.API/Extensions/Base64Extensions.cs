using System;
using System.Text;

namespace FutuCalc.API.Extensions
{
    public static class Base64Extensions
    {
        public static string PadForBase64(this string encoded)
        {
            var paddedLength = encoded.Length + 4 - (encoded.Length % 4);
            return encoded.PadRight(paddedLength, '=');
        }

        public static bool IsValidBase64(this string encoded)
        {
            var buffer = new Span<byte>(new byte[encoded.Length]);
            return Convert.TryFromBase64String(encoded, buffer, out _);
        }

        public static string DecodeBase64(this string encoded)
        {
            var decodedBytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}