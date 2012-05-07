using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class MailDAO : DAO<Mail>, IMailRepository
    {
        public MailDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}