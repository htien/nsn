using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NSN.Common;

namespace NSN.Installer
{
    public class ControllersInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyNamed(Globals.ASSEMBLY_NSN_WEBSITE)
                        .BasedOn<IController>()
                        .LifestyleTransient());
        }

        #endregion
    }
}