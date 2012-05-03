using Castle.Facilities.EventWiring;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NewSocialNetwork.Website.Core;

namespace NewSocialNetwork.Website.Installers
{
    public class StandardInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Facilities
            container.AddFacility<StartableFacility>(f => f.DeferredStart());
            container.AddFacility<EventWiringFacility>();
            //container.AddFacility<QuartzFacility>(); // Đã đăng ký trong CastleWindsor.xml

            // Singleton components
            container.Register(Component.For<NSNConfig>().LifestyleSingleton());
            container.Register(Component.For<ISessionManager>().ImplementedBy<SessionManager>().LifestyleSingleton());
        }

        #endregion
    }
}