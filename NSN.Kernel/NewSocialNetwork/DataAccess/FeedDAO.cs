using System;
using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;
using NSN.NewSocialNetwork.Domain;

namespace NewSocialNetwork.DataAccess
{
    public class FeedDAO : DAO<Feed>, IFeedRepository
    {
        public IUserRepository userRepo { private get; set; }

        public FeedDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region IFeedRepository Members

        /// <summary>
        /// Add UserTweet.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userId"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public long AddForUserTweet(int itemId, int userId, int timestamp)
        {
            return Convert.ToInt64(this.Session().CreateSQLQuery(
                    @"insert into [NSN.Feed] (TypeId, ItemId, UserId, Timestamp)
                      values (:typeId, :itemId, :userId, :timestamp); select scope_identity()")
                .SetString("typeId", NSNType.USER_TWEET)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
        }

        public IList<Feed> GetUserFeeds(int userId)
        {
            string hql = @"select f from Feed f where f.User.UserId = :userId";
            return this.Session().CreateQuery(hql)
                .SetInt32("userId", userId)
                .List<Feed>();
        }

        public IList<Feed> GetUserFeeds(int userId, int start, int size)
        {
            IList list = this.Session().CreateSQLQuery(
                    @"select FeedId, Privacy, TypeId, ItemId, ParentUserId, [Timestamp]
                      from [NSN.Feed] f where f.UserId = :userId order by f.Timestamp
                      offset :start rows fetch next :size rows only")
                .SetInt32("userId", userId)
                .SetInt32("start", start)
                .SetInt32("size", size)
                .List();

            // Xử lý kết quả
            IList<Feed> feeds = new List<Feed>();
            foreach (object[] o in list)
            {
                Feed feed = new Feed();
                feed.FeedId = Convert.ToInt64(o[0]);
                feed.Privacy = Convert.ToByte(o[1]);
                feed.TypeId = Convert.ToString(o[2]);
                feed.ItemId = Convert.ToInt32(o[3]);
                feed.ParentUser = Convert.ToInt32(o[4]) == 0 ? null : userRepo.FindById(Convert.ToInt32(o[4]));
                feed.Timestamp = Convert.ToInt32(o[5]);
                feeds.Add(feed);
            }
            return feeds;
        }

        #endregion
    }
}