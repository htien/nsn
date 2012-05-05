using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CommentTextDAO : DAO<CommentText>, ICommentTextRepository
    {
        public CommentTextDAO() { }
    }
}