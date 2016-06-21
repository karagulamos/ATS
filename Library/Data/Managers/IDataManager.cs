using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Library.Data.Managers
{
    public interface IDataManager<T> : IDataRepository
    {
        T Save(T entity);

        void Delete(T entity);

        void Update(T entity);

        void DetachAndUpdate(int id, T updatedEntity);

        int GetDataCount();
        int GetDataCount(Expression<Func<T, bool>> predicate);

        T Get(int id);

        List<T> GetAll();
        IEnumerable<T> DbFilter(Expression<Func<T, bool>> filter);

        void SaveAll(IEnumerable<T> list);

        T UpdateAll(Expression<Func<T, bool>> queryExpression, T update);
    }
}
