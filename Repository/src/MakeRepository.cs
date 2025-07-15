using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class MakeRepository(IMonoDbContext dbContext) :
    DefaultRepository<VehicleMake>(dbContext, ctx => ctx.VehicleMakes()),
    IMakeRepository<VehicleMake>;