using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NewSocialNetwork.Website.Modules.SSO;
using System;
using NewSocialNetwork.Website.Core;
using NewSocialNetwork.Website.Main;

namespace NewSocialNetwork.Website.Installers
{
    public class ModulesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            NSNConfig config = container.Resolve<NSNConfig>();
            RegisterLoginValidator(container, config);
        }

        private void RegisterLoginValidator(IWindsorContainer container, NSNConfig config)
        {
            Type loginValidator = Type.GetType(config[CfgKeys.LOGIN_AUTHENTICATOR]);
            container.Register(Component.For<ILoginAuthenticator>().ImplementedBy(loginValidator));
        }

        #endregion
    }
}