using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CommentDAO : DAO<Comment>, ICommentRepository
    {
        public CommentDAO() { }
    }
}