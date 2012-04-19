using System;
using SaberLily.PowerShell;
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
            Console.WriteLine(PublicKeyToken.Get(@"D:\workspace\HelloWorld\packages\Castle.Activerecord.3.0.0.1\lib\Net40\Castle.ActiveRecord.dll"));
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
