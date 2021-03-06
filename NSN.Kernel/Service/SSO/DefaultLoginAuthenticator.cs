﻿using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using SaberLily.Security.Crypto;
using SaberLily.Utils;

namespace NSN.Service.SSO
{
    public class DefaultLoginAuthenticator : ILoginAuthenticator
    {
        public IUserRepository userRepo { private get; set; }

        public DefaultLoginAuthenticator() { }

        #region ILoginAuthenticator Members

        public User AuthenticateUser(string usernameOrEmail, string passwd)
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