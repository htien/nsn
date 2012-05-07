using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class MailTextDAO : DAO<MailText>, IMailTextRepository
    {
        public MailTextDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}