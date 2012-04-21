using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CountryChildDAO : DAO<CountryChild>, ICountryChildRepository
    {
        public CountryChildDAO() { }
    }
}