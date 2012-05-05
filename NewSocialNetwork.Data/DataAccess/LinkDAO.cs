using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class LinkDAO : DAO<Link>, ILinkRepository
    {
        public LinkDAO() { }
    }
}