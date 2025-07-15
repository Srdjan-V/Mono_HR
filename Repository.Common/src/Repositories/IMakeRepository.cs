using Mono.Model.Common;

namespace Mono.Repository.Common.Repositories;

public interface IMakeRepository<TM> : IRepository<TM> where TM : IVehicleMake;