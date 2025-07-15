using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Repository.Common.Repositories;

namespace Mono.Repository;

public class RegistrationsRepository(IMonoDbContext dbContext) :
    DefaultRepository<VehicleRegistration>(dbContext, ctx => ctx.Registrations()),
    IRegistrationsRepository<VehicleRegistration>;