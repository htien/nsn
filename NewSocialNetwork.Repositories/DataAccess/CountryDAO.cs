using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CountryDAO : DAO<Country>, CountryRepository
    {
        public CountryDAO() { }
    }
}