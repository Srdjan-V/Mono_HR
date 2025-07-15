using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class ModelRepository(IMonoDbContext dbContext) :
    DefaultRepository<VehicleModel>(dbContext, ctx => ctx.VehicleModels()),
    IModelRepository<VehicleModel>;