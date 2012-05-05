using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NSN.Kernel.Part.Services;

namespace NSN.Kernel.Part.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn<IBusinessService>().LifestyleSingleton());
        }

        #endregion
    }
}