using System.Linq.Expressions;

namespace DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithQueryAsync(Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T?> GetByFirstOrDefaultAsync(Func<IQueryable<T>, IQueryable<T>>? query = null);
        Task<List<TResult>> GetSelectedAsync<TResult>(Expression<Func<T, TResult>> selector);
        Task SaveChangesAsync();
        void UpdateRange(IEnumerable<T> entities);
    }
}
