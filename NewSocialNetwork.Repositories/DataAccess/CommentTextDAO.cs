using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CommentTextDAO : DAO<CommentText>, CommentTextRepository
    {
        public CommentTextDAO() { }
    }
}