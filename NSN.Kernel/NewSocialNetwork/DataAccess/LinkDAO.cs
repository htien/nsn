using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LinkDAO : DAO<Link>, ILinkRepository
    {
        public LinkDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}