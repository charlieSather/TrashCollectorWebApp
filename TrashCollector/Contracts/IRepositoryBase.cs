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
        IQueryable<T> FindByConditionWithInclude<TEntity1, TEntity2>(Expression<Func<T, bool>> expression, Expression<Func<T, TEntity1>> entityOneExpression, Expression<Func<T, TEntity2>> entityTwoExpression);
        IQueryable<T> FindByConditionWithInclude<TEntity>(Expression<Func<T, bool>> expression, Expression<Func<T, TEntity>> entityOneExpression);

        void Create(T entity); 
        void Update(T entity); 
        void Delete(T entity);

    }
}
