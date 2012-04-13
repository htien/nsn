using NewSocialNetwork.Entities;

namespace NewSocialNetwork.Repositories
{
    /// <summary>
    /// Định nghĩa thêm những phương thức truy vấn (lớp DAO)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : INSNEntity
    {
        T Save(T entity);
        void Delete(T entity);
    }
}