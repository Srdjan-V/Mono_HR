using Mono.DAL;
using Mono.Model.Common;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class ModelRepository(IMonoDbContext dbContext)
    : DefaultRepository<IVehicleModel>(dbContext, ctx => ctx.VehicleModels()), IModelRepository;