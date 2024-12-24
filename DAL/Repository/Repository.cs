using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> GetAllWithQueryAsync(
            Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            IQueryable<T> baseQuery = _dbSet;

            if (query != null)
            {
                baseQuery = query(baseQuery);
            }

            return await baseQuery.ToListAsync();
        }

        public async Task<T?> GetByFirstOrDefaultAsync(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            IQueryable<T> baseQuery = _dbSet;

            if (query != null)
            {
                baseQuery = query(baseQuery);
            }

            return await baseQuery.FirstOrDefaultAsync();
        }

        public async Task<List<TResult>> GetSelectedAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbSet.Select(selector).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }
    }
}
