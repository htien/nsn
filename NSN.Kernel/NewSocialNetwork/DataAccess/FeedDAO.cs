using System;
using System.Collections;
using System.Collections.Generic;
using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

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
        public long Add(string typeId, int itemId, int userId, int parentUserId, int timestamp)
        {
            return Convert.ToInt64(this.Session().CreateSQLQuery(
                    @"insert into [NSN.Feed] (TypeId, ItemId, UserId, ParentUserId, Timestamp)
                      values (:typeId, :itemId, :userId, :parentUserId, :timestamp); select scope_identity()")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .SetInt32("userId", userId)
                .SetInt32("parentUserId", parentUserId)
                .SetInt32("timestamp", timestamp)
                .UniqueResult());
        }

        public IList<Feed> GetUserFeeds(int userId)
        {
            return GetUserFeeds(userId, false);
        }

        public IList<Feed> GetUserFeeds(int userId, bool onlyme)
        {
            string hql = "";
            if (onlyme)
            {
                hql = @"select f from Feed f where f.User.UserId = :userId where f.ParentUser.UserId = 0";
            }
            else
            {
                hql = @"select f from Feed f where f.User.UserId = :userId";
            }            
            return this.Session().CreateQuery(hql)
                .SetInt32("userId", userId)
                .List<Feed>();
        }

        public Feed GetFeed(long feedId)
        {
            object[] o =  (object[])this.Session().CreateSQLQuery(
                @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                  from [NSN.Feed] f where f.FeedId = :feedId")
                .SetInt64("feedId", feedId)
                .UniqueResult();
            return new Feed()
            {
                FeedId = Convert.ToInt64(o[0]),
                Privacy = Convert.ToByte(o[1]),
                TypeId = Convert.ToString(o[2]),
                ItemId = Convert.ToInt32(o[3]),
                User = userRepo.FindById(Convert.ToInt32(o[4])),
                ParentUser = Convert.ToInt32(o[5]) == 0 ? null : userRepo.FindById(Convert.ToInt32(o[5])),
                Timestamp = Convert.ToInt32(o[6])
            };
        }

        public IList<Feed> GetUserFeeds(int userId, int start, int size)
        {
            return GetUserFeeds(userId, start, size, true);
        }

        public IList<Feed> GetUserFeeds(int userId, int start, int size, bool onlyme)
        {
            string hql = "";
            if (onlyme)
            {
                hql = @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                        from [NSN.Feed] f
                        where (f.UserId = :userId and f.ParentUserId = 0) or (f.ParentUserId = :userId)
                        order by f.Timestamp desc
                        offset :start rows fetch next :size rows only";
            }
            else
            {
                hql = @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                        from [NSN.Feed] f
                        where f.UserId = :userId or f.ParentUserId = :userId
                        order by f.Timestamp desc
                        offset :start rows fetch next :size rows only";
            }
            IList list = this.Session().CreateSQLQuery(hql)
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
                feed.User = userRepo.FindById(Convert.ToInt32(o[4]));
                feed.ParentUser = Convert.ToInt32(o[5]) == 0 ? null : userRepo.FindById(Convert.ToInt32(o[5]));
                feed.Timestamp = Convert.ToInt32(o[6]);
                feeds.Add(feed);
            }
            return feeds;
        }

        #endregion
    }
}