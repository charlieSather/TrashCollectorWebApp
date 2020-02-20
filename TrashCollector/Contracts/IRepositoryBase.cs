using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrashCollector.Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionWithInclude<K, P>(Expression<Func<T, bool>> expression, Expression<Func<T, K>> addressExpression, Expression<Func<T, P>> pickupExpression);
        //IQueryable<T> FindByConditionWithInclude(Expression<Func<T, bool>> expression, string address,string );
        void Create(T entity); 
        void Update(T entity); 
        void Delete(T entity);

    }
}
