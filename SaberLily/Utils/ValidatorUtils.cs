using System.Net.Mail;

namespace SaberLily.Utils
{
    public static class ValidatorUtils
    {
        public static bool IsEmail(string emailAddress)
        {
            try
            {
                MailAddress email = new MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}