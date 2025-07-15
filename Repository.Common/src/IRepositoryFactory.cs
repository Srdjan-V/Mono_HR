using Mono.Model.Common;

namespace Mono.Repository.Common;

//using an factory call for each unit of work https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
public interface IRepositoryFactory<T> where T : IBaseEntity
{
    public IRepository<T> Build();
}