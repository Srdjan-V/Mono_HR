using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class EngineTypeRepository(IMonoDbContext dbContext) :
    DefaultRepository<VehicleEngineType>(dbContext, ctx => ctx.EngineTypes()),
    IEngineTypeRepository<VehicleEngineType>;