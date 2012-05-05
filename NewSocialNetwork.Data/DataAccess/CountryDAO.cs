using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CountryDAO : DAO<Country>, ICountryRepository
    {
        public CountryDAO() { }
    }
}