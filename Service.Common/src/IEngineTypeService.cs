using Mono.Model;
using Mono.Repository.Common;

namespace Mono.Service.Common;

public interface IEngineTypeService
{
    public void Initialize(IRepositoryFactory<VehicleEngineType> factory);
}