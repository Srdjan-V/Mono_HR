using Mono.Model.Common;

namespace Mono.Repository.Common.Repositories;

public interface IModelRepository<TM> : IRepository<TM> where TM : IVehicleModel;