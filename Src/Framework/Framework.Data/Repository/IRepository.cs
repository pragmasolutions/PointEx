using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Data.Helpers;

namespace Framework.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> whereClause,params Expression<Func<T, object>>[] includes);
        PagedResultList<T> GetAll(PagingCriteria paging, params Expression<Func<T, object>>[] includes);
        PagedResultList<T> GetAll(PagingCriteria paging, Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);
        T Get(int id);
        T Get(Expression<Func<T, bool>> whereClause, params Expression<Func<T, object>>[] includes);
        void Add(T entity);
        void Edit(T entity);
        void Delete(T entity);
        void Delete(int id);
        bool Commit();
    }
}
