using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class CountryChildDAO : DAO<CountryChild>, CountryChildRepository
    {
        public CountryChildDAO() { }
    }
}