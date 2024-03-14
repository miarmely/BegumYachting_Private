using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BegumYatch.Core.Repositories
{
    public partial interface IGenericRepository<T> where T : class
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

    public partial interface IGenericRepository<T>  // By Mert
    {
        Task<List<TEntity>> FromSqlRawAsync<TEntity>(
            string sql,
            params object[] parameters)
            where TEntity : class;

        Task ExecuteSqlRawAsync(
            string sql,
            params object[] parameters);
    }
}
