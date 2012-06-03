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

        public int Remove(long feedId)
        {
            return this.Session().CreateQuery(
                @"delete from Feed where FeedId = :feedId")
                .SetInt64("feedId", feedId)
                .ExecuteUpdate();
        }

        public int Remove(string typeId, int itemId)
        {
            return this.Session().CreateQuery(
                @"delete from Feed where TypeId = :typeId and ItemId = :itemId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .ExecuteUpdate();
        }

        public IList<Feed> ListStreamByUser(int userId, int start = 0, int size = 25)
        {
            IList listFeeds = this.Session().CreateSQLQuery(
                @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                  from [NSN.Feed] f
                  where f.UserId = :userId or
                        f.UserId in (select fr.FriendUserId from [NSN.Friend] fr
                                        where fr.UserId = :userId)
                  order by Timestamp desc
                  offset :start rows fetch next :size rows only")
                .SetInt32("userId", userId)
                .SetInt32("start", start)
                .SetInt32("size", size)
                .List();
            return LoadFeeds(listFeeds);
        }

        public IList<Feed> ListNewestFeed(int userId, long lastFeedId, int start = 0, int size = 5)
        {
            IList listFeeds = this.Session().CreateSQLQuery(
                @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                  from [NSN.Feed] f
                  where f.FeedId > :lastFeedId and
                        (f.UserId = :userId or
                         f.UserId in (select fr.FriendUserId from [NSN.Friend] fr where fr.UserId = :userId))
                  order by Timestamp asc
                  offset :start rows fetch next :size rows only")
                .SetInt32("userId", userId)
                .SetInt64("lastFeedId", lastFeedId)
                .SetInt32("start", start)
                .SetInt32("size", size)
                .List();
            return LoadFeeds(listFeeds);
        }

        public IList<Feed> ListFeedsByItem(string typeId, int itemId)
        {
            return this.Session().CreateQuery(
                @"from Feed where TypeId = :typeId and ItemId = :itemId")
                .SetString("typeId", typeId)
                .SetInt32("itemId", itemId)
                .List<Feed>();
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
            object[] objFeed =  (object[])this.Session().CreateSQLQuery(
                @"select FeedId, Privacy, TypeId, ItemId, UserId, ParentUserId, [Timestamp]
                  from [NSN.Feed] f where f.FeedId = :feedId")
                .SetInt64("feedId", feedId)
                .UniqueResult();
            return LoadFeed(objFeed);
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
            return LoadFeeds(list);
        }

        public bool IsItemOfUser(string typeId, int itemId, int userId)
        {
            string hql = null;
            switch (typeId)
            {
                case NSNType.PHOTO_ALBUM:
                case NSNType.PHOTO_ALBUM_MORE:
                    hql = @"select count(AlbumId) from PhotoAlbum where AlbumId = :itemId and User.UserId = :userId";
                    break;
                case NSNType.PHOTO:
                case NSNType.PHOTO_FRIEND:
                    hql = @"select count(PhotoId) from Photo
                            where PhotoId = :itemId and (FriendUser.UserId = :userId or User.UserId = :userId)";
                    break;
                case NSNType.USER_TWEET:
                    hql = @"select count(TweetId) from UserTweet
                            where TweetId = :itemId and (FriendUser.UserId = :userId or User.UserId = :userId)";
                    break;
                case NSNType.LINK:
                case NSNType.LINK_FRIEND:
                    hql = @"select count(LinkId) from Link
                            where LinkId = :itemId and (FriendUser.UserId = :userId or User.UserId = :userId)";
                    break;
            }
            if (!String.IsNullOrEmpty(hql))
            {
                return Convert.ToInt32(this.Session().CreateQuery(hql)
                    .SetInt32("itemId", itemId)
                    .SetInt32("userId", userId)
                    .UniqueResult()) > 0;
            }
            return false;
        }

        #endregion

        private Feed LoadFeed(object[] objFeed)
        {
            return new Feed()
            {
                FeedId = Convert.ToInt64(objFeed[0]),
                Privacy = Convert.ToByte(objFeed[1]),
                TypeId = Convert.ToString(objFeed[2]),
                ItemId = Convert.ToInt32(objFeed[3]),
                User = userRepo.FindById(Convert.ToInt32(objFeed[4])),
                ParentUser = Convert.ToInt32(objFeed[5]) == 0 ? null : userRepo.FindById(Convert.ToInt32(objFeed[5])),
                Timestamp = Convert.ToInt32(objFeed[6])
            };
        }

        private IList<Feed> LoadFeeds(IList list)
        {
            IList<Feed> feeds = new List<Feed>();
            foreach (object[] o in list)
            {
                Feed feed = LoadFeed(o);
                feeds.Add(feed);
            }
            return feeds;
        }
    }
}