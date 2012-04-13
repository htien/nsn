using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CommentDAO : DAO<Comment>, CommentRepository
    {
        public CommentDAO() { }
    }
}