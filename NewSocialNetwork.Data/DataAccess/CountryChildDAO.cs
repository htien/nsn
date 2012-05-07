using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CountryChildDAO : DAO<CountryChild>, ICountryChildRepository
    {
        public CountryChildDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }
    }
}