using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NSN.Framework;
using NSN.Security.SSO;

namespace NSN.Kernel.Part.Installers
{
    public class ModulesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            INSNConfig config = container.Resolve<INSNConfig>();
            RegisterLoginValidator(container, config);
        }

        private void RegisterLoginValidator(IWindsorContainer container, INSNConfig config)
        {
            Type loginValidator = Type.GetType(config[CfgKeys.LOGIN_AUTHENTICATOR]);
            container.Register(Component.For<ILoginAuthenticator>().ImplementedBy(loginValidator).LifestyleSingleton());
        }

        #endregion
    }
}