using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrashCollector.Data;

namespace TrashCollector.Contracts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        public RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }
        public IQueryable<T> FindAll()
        {
            return ApplicationDbContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return ApplicationDbContext.Set<T>().Where(expression).AsNoTracking();
        }
        public IQueryable<T> FindByConditionWithInclude<K,P>(Expression<Func<T, bool>> expression, Expression<Func<T,K>> addressExpression, Expression<Func<T,P>> pickupExpression)
        {
            return ApplicationDbContext.Set<T>().Include(addressExpression).Include(pickupExpression).Where(expression).AsNoTracking();
        }
        //public IQueryable<T> FindByConditionWithInclude(Expression<Func<T, bool>> expression, string address, string pickup)
        //{
        //    return ApplicationDbContext.Set<T>().Include(expression).Include(pickup).Where(expression).AsNoTracking();
        //}

        public void Create(T entity)
        {
            ApplicationDbContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            ApplicationDbContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            ApplicationDbContext.Set<T>().Remove(entity);
        }
  
    }
}
