using Mono.Model.Common;

namespace Mono.Repository.Common.Repositories;

public interface IOwnerRepository<TM> : IRepository<TM> where TM : IVehicleOwner;