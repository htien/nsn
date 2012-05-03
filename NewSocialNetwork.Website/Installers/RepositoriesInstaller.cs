using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NewSocialNetwork.DataAccess;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.Website.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Component.For<ICommentRepository>().ImplementedBy<CommentDAO>())
                .Register(Component.For<ICommentTextRepository>().ImplementedBy<CommentTextDAO>())
                .Register(Component.For<ICountryChildRepository>().ImplementedBy<CountryChildDAO>())
                .Register(Component.For<ICountryRepository>().ImplementedBy<CountryDAO>())
                .Register(Component.For<ICustomRelationDataRepository>().ImplementedBy<CustomRelationDataDAO>())
                .Register(Component.For<ICustomRelationRepository>().ImplementedBy<CustomRelationDAO>())
                .Register(Component.For<IEmotionPackageRepository>().ImplementedBy<EmotionPackageDAO>())
                .Register(Component.For<IEmotionRepository>().ImplementedBy<EmotionDAO>())
                .Register(Component.For<IFeedRepository>().ImplementedBy<FeedDAO>())
                .Register(Component.For<IFriendListDataRepository>().ImplementedBy<FriendListDataDAO>())
                .Register(Component.For<IFriendListRepository>().ImplementedBy<FriendListDAO>())
                .Register(Component.For<IFriendRepository>().ImplementedBy<FriendDAO>())
                .Register(Component.For<IFriendRequestRepository>().ImplementedBy<FriendRequestDAO>())
                .Register(Component.For<ILikeCacheRepository>().ImplementedBy<LikeCacheDAO>())
                .Register(Component.For<ILikeRepository>().ImplementedBy<LikeDAO>())
                .Register(Component.For<ILinkRepository>().ImplementedBy<LinkDAO>())
                .Register(Component.For<IMailFolderRepository>().ImplementedBy<MailFolderDAO>())
                .Register(Component.For<IMailRepository>().ImplementedBy<MailDAO>())
                .Register(Component.For<IMailTextRepository>().ImplementedBy<MailTextDAO>())
                .Register(Component.For<IPhotoAlbumInfoRepository>().ImplementedBy<PhotoAlbumInfoDAO>())
                .Register(Component.For<IPhotoAlbumRepository>().ImplementedBy<PhotoAlbumDAO>())
                .Register(Component.For<IPhotoInfoRepository>().ImplementedBy<PhotoInfoDAO>())
                .Register(Component.For<IPhotoRepository>().ImplementedBy<PhotoDAO>())
                .Register(Component.For<ISessionRepository>().ImplementedBy<SessionDAO>())
                .Register(Component.For<IUserCountRepository>().ImplementedBy<UserCountDAO>())
                .Register(Component.For<IUserGroupRepository>().ImplementedBy<UserGroupDAO>())
                .Register(Component.For<IUserRepository>().ImplementedBy<UserDAO>())
                .Register(Component.For<IUserTweetRepository>().ImplementedBy<UserTweetDAO>())
                .Register(Component.For<INSNVersionRepository>().ImplementedBy<VersionDAO>());
        }

        #endregion
    }
}