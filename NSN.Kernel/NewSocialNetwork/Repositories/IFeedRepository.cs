using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface IFeedRepository : IRepository<Feed>
    {
        IList<Feed> GetUserFeeds(int userId);
    }
}