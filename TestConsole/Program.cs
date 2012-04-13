using System;
using SaberLily.Security.Crypto;
using SaberLily.Utils;

namespace NewSocialNetwork.TestConsole
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Current timestamp: " + GetTimestamp());
        }

        public static string HashPassword(string text)
        {
            return PasswordCryptor.Hash(text, 690);
        }

        public static int GetTimestamp()
        {
            return DateTimeUtils.UnixTimestamp;
        }
    }
}
