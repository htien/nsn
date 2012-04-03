using System;
using System.Text;
using System.Security.Cryptography;

namespace SaberLily.Security.Crypto
{
    /// <summary>
    /// A salted password hashing library.
    /// </summary>
    public static class PasswordCryptor
    {
        public static string Hash(string text, int saltSize)
        {
            saltSize = saltSize < 0 ? 0 : saltSize;

            string salt = CreateRandomSalt(saltSize);
            string hash = SHA512Base64(new StringBuilder()
                    .Append(salt).Append(text).Append(salt)
                    .ToString());
            return new StringBuilder()
                .Append("(=^o^=)").Append(salt).Append(hash).Append("(=^o^=)")
                .ToString();
        }

        public static bool Verify(string text, string hashedText)
        {
            if (hashedText.Length < 88 + 14)
            {
                throw new ArgumentException("hashedText must be greater than 88 characters.");
            }
            string salt = hashedText.Substring(7, hashedText.Length - 88 - 14);
            string origHash = hashedText.Substring(salt.Length + 7, 88);
            string newHash = SHA512Base64(salt + text + salt);
            return string.Compare(origHash, newHash, StringComparison.Ordinal) == 0;
        }

        private static string SHA512Hex(string toHash)
        {
            SHA512Managed sha = new SHA512Managed();
            byte[] utf8 = UTF8Encoding.UTF8.GetBytes(toHash);
            return BytesToHex(sha.ComputeHash(utf8));
        }

        private static string SHA512Base64(string toHash)
        {
            SHA512Managed sha = new SHA512Managed();
            byte[] utf8 = UTF8Encoding.UTF8.GetBytes(toHash);
            return Convert.ToBase64String(sha.ComputeHash(utf8));
        }

        /// <summary>
        /// Return a Base64 string representation of the random number.
        /// </summary>
        /// <param name="size">number of bytes</param>
        /// <returns>a Base64 string</returns>        
        private static string CreateRandomSalt(int size)
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[size];
            random.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Convert a byte array to a hex string.
        /// </summary>
        /// <param name="toConvert">a byte array</param>
        /// <returns>a hex string</returns>
        private static string BytesToHex(byte[] toConvert)
        {
            StringBuilder sb = new StringBuilder(toConvert.Length * 2);
            foreach (byte b in toConvert)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
