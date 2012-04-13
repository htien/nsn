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
                .Register(Component.For<CommentRepository>().ImplementedBy<CommentDAO>())
                .Register(Component.For<CommentTextRepository>().ImplementedBy<CommentTextDAO>())
                .Register(Component.For<CountryChildRepository>().ImplementedBy<CountryChildDAO>())
                .Register(Component.For<CountryRepository>().ImplementedBy<CountryDAO>())
                .Register(Component.For<CustomRelationDataRepository>().ImplementedBy<CustomRelationDataDAO>())
                .Register(Component.For<CustomRelationRepository>().ImplementedBy<CustomRelationDAO>())
                .Register(Component.For<EmotionPackageRepository>().ImplementedBy<EmotionPackageDAO>())
                .Register(Component.For<EmotionRepository>().ImplementedBy<EmotionDAO>())
                .Register(Component.For<FeedRepository>().ImplementedBy<FeedDAO>())
                .Register(Component.For<FriendListDataRepository>().ImplementedBy<FriendListDataDAO>())
                .Register(Component.For<FriendListRepository>().ImplementedBy<FriendListDAO>())
                .Register(Component.For<FriendRepository>().ImplementedBy<FriendDAO>())
                .Register(Component.For<FriendRequestRepository>().ImplementedBy<FriendRequestDAO>())
                .Register(Component.For<LikeCacheRepository>().ImplementedBy<LikeCacheDAO>())
                .Register(Component.For<LikeRepository>().ImplementedBy<LikeDAO>())
                .Register(Component.For<LinkRepository>().ImplementedBy<LinkDAO>())
                .Register(Component.For<MailFolderRepository>().ImplementedBy<MailFolderDAO>())
                .Register(Component.For<MailRepository>().ImplementedBy<MailDAO>())
                .Register(Component.For<MailTextRepository>().ImplementedBy<MailTextDAO>())
                .Register(Component.For<PhotoAlbumInfoRepository>().ImplementedBy<PhotoAlbumInfoDAO>())
                .Register(Component.For<PhotoAlbumRepository>().ImplementedBy<PhotoAlbumDAO>())
                .Register(Component.For<PhotoInfoRepository>().ImplementedBy<PhotoInfoDAO>())
                .Register(Component.For<PhotoRepository>().ImplementedBy<PhotoDAO>())
                .Register(Component.For<UserCountRepository>().ImplementedBy<UserCountDAO>())
                .Register(Component.For<UserGroupRepository>().ImplementedBy<UserGroupDAO>())
                .Register(Component.For<UserRepository>().ImplementedBy<UserDAO>())
                .Register(Component.For<UserTweetRepository>().ImplementedBy<UserTweetDAO>())
                .Register(Component.For<NSNVersionRepository>().ImplementedBy<VersionDAO>());
        }

        #endregion
    }
}