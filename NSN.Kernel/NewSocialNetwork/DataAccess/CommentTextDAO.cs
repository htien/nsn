using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CommentTextDAO : DAO<CommentText>, ICommentTextRepository
    {
        public CommentTextDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}