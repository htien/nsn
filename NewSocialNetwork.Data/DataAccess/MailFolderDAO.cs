using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    public class MailFolderDAO : DAO<MailFolder>, IMailFolderRepository
    {
        public MailFolderDAO() { }
    }
}