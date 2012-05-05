using System.Collections.Generic;
using System.Transactions;
using Castle.ActiveRecord;
using Castle.Services.Transaction;
using NHibernate.Criterion;
using NSN.Framework;

namespace NewSocialNetwork.DataAccess
{
    /// <summary>
    /// Những phương thức truy vấn chung cho mọi NSNEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DAO<T> : IRepository<T> where T : class, IEntity
    {
        public DAO() { }

        #region IRepository<T> Members

        [Transaction(TransactionScopeOption.Required)]
        public T Create(T entity)
        {
            ActiveRecordMediator<T>.Create(entity);
            return entity;
        }

        [Transaction(TransactionScopeOption.Required)]
        public T Save(T entity)
        {
            ActiveRecordMediator<T>.Save(entity);
            return entity;
        }

        [Transaction(TransactionScopeOption.Required)]
        public void Delete(T entity)
        {
            ActiveRecordMediator<T>.Delete(entity);
        }

        public T FindById(object id)
        {
            return ActiveRecordMediator<T>.FindByPrimaryKey(id);
        }

        public T FindById(object id, bool throwOnNotFound)
        {
            return ActiveRecordMediator<T>.FindByPrimaryKey(id, throwOnNotFound);
        }

        public IList<T> FindAll()
        {
            return ActiveRecordMediator<T>.FindAll();
        }

        public IList<T> FindAll(Order order, params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.FindAll(new[] { order }, criteria);
        }

        public IList<T> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            return ActiveRecordMediator<T>.FindAll(criteria, orders);
        }

        public IList<T> FindAll(Order[] orders, params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.FindAll(orders, criteria);
        }

        public IList<T> FindAll(params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.FindAll(criteria);
        }

        public IList<T> FindAllByProperty(string property, object value)
        {
            return ActiveRecordMediator<T>.FindAllByProperty(property, value);
        }

        public IList<T> SlicedFind(int firstResult, int maxResults, Order[] orders, params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.SlicedFindAll(firstResult, maxResults, orders, criteria);
        }

        public T FindOne(params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.FindOne(criteria);
        }

        public T FindOne(DetachedCriteria criteria)
        {
            return ActiveRecordMediator<T>.FindOne(criteria);
        }

        public T FindFirst(DetachedCriteria criteria, params Order[] orders)
        {
            return ActiveRecordMediator<T>.FindFirst(criteria, orders);
        }

        public T FindFirst(params Order[] orders)
        {
            return ActiveRecordMediator<T>.FindFirst(orders);
        }

        public bool Exists()
        {
            return ActiveRecordMediator<T>.Exists();
        }

        public bool Exists(DetachedCriteria criteria)
        {
            return ActiveRecordMediator<T>.Exists(criteria);
        }

        public int Count()
        {
            return ActiveRecordMediator<T>.Count();
        }

        public int Count(params ICriterion[] criteria)
        {
            return ActiveRecordMediator<T>.Count(criteria);
        }

        public int Count(DetachedCriteria criteria)
        {
            return ActiveRecordMediator<T>.Count(criteria);
        }

        #endregion
    }
}