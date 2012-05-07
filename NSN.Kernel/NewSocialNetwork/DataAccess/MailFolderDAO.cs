using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class MailFolderDAO : DAO<MailFolder>, IMailFolderRepository
    {
        public MailFolderDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}