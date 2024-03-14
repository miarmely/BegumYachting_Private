using BegumYatch.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BegumYatch.Repository.Repositories
{
    public partial class GenericRepository<T> 
        : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {

            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(T updateEntity)
        {
            _dbSet.Update(updateEntity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => { _dbSet.Update(entity); });
            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }

    public partial class GenericRepository<T>  // By MERT
    {
        public async Task<List<TEntity>> FromSqlRawAsync<TEntity>(
            string sql,
            params object[] parameters)
            where TEntity : class =>
                await _context
                    .Set<TEntity>()
                    .FromSqlRaw(sql, parameters)
                    .ToListAsync();

        public async Task ExecuteSqlRawAsync(
            string sql,
            params object[] parameters) =>
                await _context.Database
                    .ExecuteSqlRawAsync(sql, parameters);
    }
}
