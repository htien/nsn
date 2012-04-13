using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class FeedDAO : DAO<Feed>, FeedRepository
    {
        public FeedDAO() { }
    }
}