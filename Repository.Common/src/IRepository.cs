using Mono.Model.Common;

namespace Mono.Repository.Common;

//UnitOfWork
public interface IRepository<T> : IDisposable where T : IBaseEntity
{
    Task<int> CommitAsync();

    ValueTask<T?> GetAsync(long id);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<int> DeleteAsync(long id);
}