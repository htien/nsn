using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NSN.Kernel.Manager
{
    /// <summary>
    /// Quản lý các TypeId
    /// </summary>
    public class FeedItem
    {
        public Feed _Feed { get; set; }
        public IEntity _Entity { get; set; }

        public FeedItem(Feed feed, IEntity entity)
        {
            this._Feed = feed;
            this._Entity = entity;
        }
    }
}