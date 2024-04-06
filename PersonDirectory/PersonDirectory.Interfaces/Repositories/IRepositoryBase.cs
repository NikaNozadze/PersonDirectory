using System.Linq.Expressions;

namespace PersonDirectory.Interfaces.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(params object[] id);
        IQueryable<T> Set(Expression<Func<T, bool>> predicate);
        IQueryable<T> Set();
        void Insert(T entity);
        void Update(T entity);
        void InsertOrUpdate(T entity);
        void Delete(object id);
        void Delete(T entity);
    }
}
