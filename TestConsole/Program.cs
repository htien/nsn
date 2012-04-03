using System;

using SaberLily.Security.Crypto;

namespace NewSocialNetwork.TestConsole
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Console.WriteLine("Hello World!");

            string hashed = PasswordCryptor.Hash("the gioi tu do", 690);
            bool newHashed = PasswordCryptor.Verify("the gioi tu do", hashed);

            Console.WriteLine(hashed);
            Console.WriteLine(hashed.Length);
            Console.WriteLine(newHashed);

            Console.ReadLine();
        }
    }
}
