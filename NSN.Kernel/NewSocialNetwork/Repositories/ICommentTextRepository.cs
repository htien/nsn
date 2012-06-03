using NewSocialNetwork.Domain;
using NSN.Framework;

namespace NewSocialNetwork.Repositories
{
    public interface ICommentTextRepository : IRepository<CommentText>
    {
        long Add(long commentId, string text, string textParsed);
        int Remove(int commentId);
    }
}