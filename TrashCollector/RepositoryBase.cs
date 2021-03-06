﻿using Microsoft.EntityFrameworkCore;
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
        public IQueryable<T> FindByConditionWithInclude<TEntity1, TEntity2>(Expression<Func<T, bool>> expression, Expression<Func<T, TEntity1>> entityOneExpression, Expression<Func<T, TEntity2>> entityTwoExpression)
        {
            return ApplicationDbContext.Set<T>().Include(entityOneExpression).Include(entityTwoExpression).Where(expression).AsNoTracking();
        }
        public IQueryable<T> FindByConditionWithInclude<TEntity>(Expression<Func<T, bool>> expression, Expression<Func<T, TEntity>> entityExpression)
        {
            return ApplicationDbContext.Set<T>().Include(entityExpression).Where(expression).AsNoTracking();
        }
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
