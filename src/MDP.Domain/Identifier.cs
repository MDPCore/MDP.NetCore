using System;
using System.Buffers.Text;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MDP.Domain
{
    public class Identifier
    {
        // Constants
        private static readonly char[] _baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private static readonly HashAlgorithm _algorithm = MD5.Create();


        // Methods
        public static string NewId(int length = 8)
        {
            // hashBytes
            var hashBytes = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

            // heshIntger
            var heshIntger = BitConverter.ToUInt32(hashBytes, hashBytes.Length - 4);

            // hashString
            var hashString = ConvertToBaseN(heshIntger, (uint)_baseChars.Length);
            hashString = hashString.Length >= length ? hashString : hashString.PadLeft(length, '0');

            // Return
            return hashString;
        }

        private static string ConvertToBaseN(uint number, uint baseN)
        {
            // Require
            if (baseN < 2) throw new ArgumentOutOfRangeException(nameof(baseN));
            if (baseN > _baseChars.Length) throw new ArgumentOutOfRangeException(nameof(baseN));

            // Convert
            if (number < baseN)
            {
                return _baseChars[number].ToString();
            }
            else
            {
                return ConvertToBaseN(number / baseN, baseN) + _baseChars[number % baseN];
            }
        }
    }
}
