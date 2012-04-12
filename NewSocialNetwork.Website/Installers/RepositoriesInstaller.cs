using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NewSocialNetwork.Entities;
using NewSocialNetwork.Website.Repositories;

namespace NewSocialNetwork.Website.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                    .Where(Component.IsInSameNamespaceAs<IRepository<INSNEntity>>())
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient());
        }

        #endregion
    }
}