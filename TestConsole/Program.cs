using System;
using SaberLily.PowerShell;
using SaberLily.Security.Crypto;
using SaberLily.Utils;
using System.IO;

namespace NewSocialNetwork.TestConsole
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Current timestamp: " + GetTimestamp());
            Console.WriteLine(PublicKeyToken.Get(@"D:\workspace\HelloWorld\packages\Quartz.1.0.3\lib\3.5\Quartz.dll"));
            Console.WriteLine(DateTimeUtils.CurrentTimeMillis);
            Console.ReadLine();
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
