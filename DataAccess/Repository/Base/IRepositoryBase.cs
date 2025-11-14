using System.Linq.Expressions;

namespace DataAccess.Repository.Base
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> Query(bool asNoTracking = false);
        IQueryable<T> Search(Expression<Func<T, bool>>? predicate = null, bool asNoTracking = false);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        // basic include method belki ise yarar diye ekledim.
        IQueryable<T> Include(params string[] includes);

    }
}
