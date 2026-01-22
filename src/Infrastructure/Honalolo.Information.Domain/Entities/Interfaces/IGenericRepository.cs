using Honalolo.Information.Domain.Common;
using Honalolo.Information.Domain.Entities.Attractions;
using System.Linq.Expressions;

namespace Honalolo.Information.Domain.Entities.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);

        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
