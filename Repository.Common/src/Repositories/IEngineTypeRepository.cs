using Mono.Model.Common;

namespace Mono.Repository.Common.Repositories;

public interface IEngineTypeRepository<TM> : IRepository<TM> where TM : IVehicleEngineType;