﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Auth;

namespace Backend.Core.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int Id);
        Task<TEntity> GetByUsernameAsync(string username);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        IQueryable<TEntity> IncludeMany(params Expression<Func<TEntity, object>>[] includes);
    }
}
