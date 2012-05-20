using NewSocialNetwork.Domain;
using NewSocialNetwork.Repositories;
using NHibernate;

namespace NewSocialNetwork.DataAccess
{
    public class CommentTextDAO : DAO<CommentText>, ICommentTextRepository
    {
        public CommentTextDAO(ISessionFactory sessionFactory) : base(sessionFactory)
        { }

        #region ICommentTextRepository Members

        public long Add(long commentId, string text, string textParsed)
        {
            this.Session().CreateSQLQuery(
                @"insert into [NSN.CommentText](CommentId, Text, TextParsed)
                  values (:commentId, :text, :textParsed)")
                .SetInt64("commentId", commentId)
                .SetString("text", text)
                .SetString("textParsed", textParsed)
                .ExecuteUpdate();
            return commentId;
        }

        #endregion
    }
}