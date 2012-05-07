using Castle.ActiveRecord;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate;

namespace NSN.Facility
{
    public class PersistenceFacility : AbstractFacility
    {
        protected override void Init()
        {
            var config = ActiveRecordMediator.GetSessionFactoryHolder().GetSessionFactory(typeof(ActiveRecordBase));
            Kernel.Register(Component.For<ISessionFactory>().Named("activerecord.sessionfactory").Instance(config));
        }
    }
}
