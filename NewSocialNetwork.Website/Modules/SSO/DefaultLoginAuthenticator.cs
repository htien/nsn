using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;
using SaberLily.Security.Crypto;
using SaberLily.Utils;

namespace NewSocialNetwork.Website.Modules.SSO
{
    public class DefaultLoginAuthenticator : ILoginAuthenticator
    {
        public UserRepository userRepo { get; set; }

        public DefaultLoginAuthenticator() { }

        #region ILoginValidator Members

        public User ValidateUser(string usernameOrEmail, string passwd)
        {
            // Kiểm tra phải email không?
            bool isEmail = ValidatorUtils.IsEmail(usernameOrEmail);

            // Lấy password từ database
            User user = isEmail
                ? userRepo.GetUserByEmail(usernameOrEmail)
                : userRepo.GetUserByUsername(usernameOrEmail);
            string hashedPasswd = user != null ? user.Password : null;

            if (hashedPasswd != null)
            {
                // Kiểm tra xem password có đúng không nào!
                return PasswordCryptor.Verify(passwd, hashedPasswd) ? user : null;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}