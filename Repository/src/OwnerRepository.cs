using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class OwnerRepository(IMonoDbContext dbContext) :
    DefaultRepository<VehicleOwner>(dbContext, ctx => ctx.VehicleOwners()),
    IOwnerRepository<VehicleOwner>;