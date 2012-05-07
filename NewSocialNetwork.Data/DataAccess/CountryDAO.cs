using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CountryDAO : DAO<Country>, ICountryRepository
    {
        public CountryDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}