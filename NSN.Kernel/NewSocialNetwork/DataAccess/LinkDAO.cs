using System;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class LinkDAO : DAO<Link>, ILinkRepository
    {
        public LinkDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        public int Add(int userId, int friendUserId, string content, string url, string image,
            string title, string description, int timestamp)
        {
            int linkId = Convert.ToInt32(this.Session().CreateSQLQuery(
                @"insert into [NSN.Link] (UserId, FriendUserId, [Content], Url, Image, Title, Description, Timestamp)
                  values (:userId, :friendUserId, :content, :url, :image, :title, :description, :timestamp); select scope_identity()")
                .SetInt32("userId", userId)
                .SetInt32("friendUserId", friendUserId)
                .SetString("content", content)
                .SetString("url", url)
                .SetString("image", image)
                .SetString("title", title)
                .SetString("description", description)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
            return linkId;
        }
    }
}