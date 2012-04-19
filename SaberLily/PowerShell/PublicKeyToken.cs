using System.Reflection;
using System;

namespace SaberLily.PowerShell
{
    public static class PublicKeyToken
    {
        public static string Get(string @assemblyPath)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                byte[] pt = assembly.GetName().GetPublicKeyToken(); // This give the key in a byte array
                string key = string.Empty;
                foreach (byte i in pt)
                {
                    key += string.Format("{0:x}", i); // Convert the byte-array to a human-readable string
                }
                return key;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}