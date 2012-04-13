using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class LinkDAO : DAO<Link>, LinkRepository
    {
        public LinkDAO() { }
    }
}