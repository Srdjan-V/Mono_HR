using Mono.Model.Common;

namespace Mono.Repository.Common.Repositories;

public interface IRegistrationsRepository<TM> : IRepository<TM> where TM : IVehicleRegistration;