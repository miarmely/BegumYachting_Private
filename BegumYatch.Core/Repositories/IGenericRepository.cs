﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T updateEntity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
