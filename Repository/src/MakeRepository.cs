using Mono.DAL;
using Mono.Model.Common;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class MakeRepository(IMonoDbContext dbContext)
    : DefaultRepository<IVehicleMake>(dbContext, ctx => ctx.VehicleMakes()), IMakeRepository;