﻿using Castle.Facilities.EventWiring;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NSN.Framework;
using NSN.Kernel.Manager;

namespace NSN.Kernel.Part.Installers
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
            container
                .Register(Component.For<INSNConfig>().ImplementedBy<NSNConfig>().LifestyleSingleton())
                .Register(Component.For<ISessionManager>().ImplementedBy<DefaultSessionManager>().LifestyleSingleton());
            
            NSNConfig.Instance = container.Resolve<INSNConfig>();
        }

        #endregion
    }
}