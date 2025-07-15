using Mono.Model.Common;

namespace Mono.Repository.Common;

public interface IRepositoryFactory<T> where T : IBaseEntity
{
    public IRepository<T> Build();
}