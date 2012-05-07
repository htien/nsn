using Castle.ActiveRecord;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class UserDAO : DAO<User>, IUserRepository
    {
        public UserDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region UserRepository Members

        public User GetUserByEmail(string email)
        {
            User[] users = ActiveRecordMediator<User>.FindAllByProperty("Email", email);
            return users.Length > 0 ? users[0] : null;
        }

        public User GetUserByUsername(string username)
        {
            User[] users = ActiveRecordMediator<User>.FindAllByProperty("Username", username);
            return users.Length > 0 ? users[0] : null;
        }

        public string GetPasswordByEmail(string email)
        {
            return this.GetUserByEmail(email).Password;
        }

        public string GetPasswordByUsername(string username)
        {
            return this.GetUserByUsername(username).Password;
        }

        #endregion
    }
}