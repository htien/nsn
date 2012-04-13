using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NewSocialNetwork.Website.Modules.SSO;

namespace NewSocialNetwork.Website.Installers
{
    public class ModulesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILoginAuthenticator>().ImplementedBy<DefaultLoginAuthenticator>());
        }

        #endregion
    }
}