using System;
using System.Text;

namespace FutuCalc.API.Extensions
{
    public static class Base64Extensions
    {
        /// <summary>
        /// Pads out a base-64 string to a valid length.
        /// </summary>
        /// <param name="encoded">A base-64 string which may not have a valid length.</param>
        /// <returns>
        /// The encoded string, with enough <code>=</code> characters appended
        /// so its new length is evenly divisible by 4.
        /// </returns>
        public static string PadForBase64(this string encoded)
        {
            var paddedLength = encoded.Length + 4 - (encoded.Length % 4);
            return encoded.PadRight(paddedLength, '=');
        }

        /// <summary>
        /// Checks whether or not a string can be converted from base-64.
        /// </summary>
        /// <param name="encoded">The encoded string to check.</param>
        public static bool IsValidBase64(this string encoded)
        {
            var buffer = new Span<byte>(new byte[encoded.Length]);
            return Convert.TryFromBase64String(encoded, buffer, out _);
        }

        /// <summary>
        /// Decodes a string from base-64 to UTF8.
        /// </summary>
        /// <param name="encoded">The encoded base-64 string.</param>
        public static string DecodeBase64(this string encoded)
        {
            var decodedBytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(decodedBytes);
        }
    }
}