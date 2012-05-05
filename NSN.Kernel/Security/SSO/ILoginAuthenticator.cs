using NewSocialNetwork.Domain;

namespace NSN.Security.SSO
{
    /// <summary>
    /// Giao diện cơ bản của lớp LoginValidator, dùng để xác thực người dùng
    /// chủ yếu dựa vào 'username' và 'password' nhập vào.
    /// </summary>
    public interface ILoginAuthenticator
    {
        /// <summary>
        /// Xác thực người dùng dựa vào username và password.
        /// </summary>
        /// <param name="username">Tên người dùng</param>
        /// <param name="passwd">Mật khẩu</param>
        /// <returns>Thực thể User đã đăng nhập</returns>
        User authenticateUser(string usernameOrEmail, string passwd);
    }
}