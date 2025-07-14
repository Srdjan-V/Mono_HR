using Mono.Model.Common;

namespace Mono.Repository.Common;

public interface IRepositoryFactory<in T> where T : IBaseEntity
{
    public IRepository<T> Build();
}