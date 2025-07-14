using Mono.DAL;
using Mono.Model.Common;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class OwnerRepository(IMonoDbContext dbContext)
    : DefaultRepository<IVehicleOwner>(dbContext, ctx => ctx.VehicleOwners()), IOwnerRepository;