using Mono.Model.Common;

namespace Mono.Repository.Common;

//UnitOfWork
public interface IRepository<in T> : IDisposable where T : IBaseEntity
{
    Task<int> CommitAsync();

    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<int> DeleteAsync(long id);
}