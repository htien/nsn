using System.Collections.Generic;
using NewSocialNetwork.Entities;
using NHibernate.Criterion;

namespace NewSocialNetwork.Repositories
{
    /// <summary>
    /// Định nghĩa thêm những phương thức truy vấn (lớp DAO)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : INSNEntity
    {
        T Create(T entity);
        T Save(T entity);
        void Delete(T entity);

        T FindById(object id);
        T FindById(object id, bool throwOnNotFound);
        IList<T> FindAll();
        IList<T> FindAll(Order order, params ICriterion[] criteria);
        IList<T> FindAll(DetachedCriteria criteria, params Order[] orders);
        IList<T> FindAll(Order[] orders, params ICriterion[] criteria);
        IList<T> FindAll(params ICriterion[] criteria);
        IList<T> FindAllByProperty(string property, object value);
        IList<T> SlicedFind(int firstResult, int maxResults, Order[] orders, params ICriterion[] criteria);

        T FindOne(params ICriterion[] criteria);
        T FindOne(DetachedCriteria criteria);
        T FindFirst(DetachedCriteria criteria, params Order[] orders);
        T FindFirst(params Order[] orders);

        bool Exists();
        bool Exists(DetachedCriteria criteria);

        int Count();
        int Count(params ICriterion[] criteria);
        int Count(DetachedCriteria criteria);
    }
}