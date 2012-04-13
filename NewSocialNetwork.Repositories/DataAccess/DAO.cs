using NewSocialNetwork.Entities;
using NewSocialNetwork.Repositories;

namespace NewSocialNetwork.DataAccess
{
    /// <summary>
    /// Những phương thức truy vấn chung cho mọi NSNEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DAO<T> : IRepository<T> where T : INSNEntity
    {
        public DAO() { }

        #region IRepository<T> Members

        public T Save(T entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}