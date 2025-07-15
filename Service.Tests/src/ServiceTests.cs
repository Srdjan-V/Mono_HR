using FluentAssertions;
using Mono.DAL;
using Mono.Model;
using Mono.Model.Common;
using Mono.Repository;
using Mono.Repository.Common;
using Mono.Service.Common;
using Ninject;
using Ninject.Extensions.Factory;
using Xunit;

namespace Mono.Service.Tests;

//todo implement more tests
public class ServiceTests
{
    private readonly IVehicleService vehicleService;

    private KernelBase kernel;

    public ServiceTests()
    {
        kernel = new StandardKernel();
        kernel.Bind<IRepositoryFactory<VehicleMake>>().ToFactory();
        kernel.Bind<IRepository<VehicleMake>>().To<MakeRepository>();

        kernel.Bind<IRepositoryFactory<VehicleModel>>().ToFactory();
        kernel.Bind<IRepository<VehicleModel>>().To<ModelRepository>();

        kernel.Bind<IRepositoryFactory<VehicleOwner>>().ToFactory();
        kernel.Bind<IRepository<VehicleOwner>>().To<OwnerRepository>();

        kernel.Bind<IRepositoryFactory<VehicleRegistration>>().ToFactory();
        kernel.Bind<IRepository<VehicleRegistration>>().To<RegistrationsRepository>();

        kernel.Bind<IRepositoryFactory<VehicleEngineType>>().ToFactory();
        kernel.Bind<IRepository<VehicleEngineType>>().To<EngineTypeRepository>();

        kernel.Bind<IMonoDbContextFactory>().ToFactory();
        kernel.Bind<IMonoDbContext>().To<InMemorySqliteMonoDbContext>().InSingletonScope();

        kernel.Bind<IVehicleService>().To<VehicleService>();

        vehicleService = kernel.Get<IVehicleService>();
    }

    [Fact]
    private async void RegisterCreate()
    {
        using (var repository = kernel.Get<IRepositoryFactory<VehicleEngineType>>().Build())
        {
            await repository.AddAsync(new VehicleEngineType
            {
                Id = 10,
                Type = "2CLD",
                Abrv = "2CLD"
            });
            await repository.CommitAsync();
        }

        //todo use dto to skip required fields
        IVehicleRegistration registration = await vehicleService.RegisterVehicleAsync(
            new VehicleRegistration
            {
                Id = 0,
                RegistrationNumber = "SO 200",
                VehicleModelId = 0,
                VehicleEngineTypeId = 0,
                VehicleOwnerId = 0
            }, new VehicleModel
            {
                Id = 0,
                Name = "x5",
                Abrv = "x5",
                VehicleMakeId = 0
            }, new VehicleMake
            {
                Id = 0,
                Name = "BMW",
                Abrv = "BMW"
            }, new VehicleEngineType
            {
                Id = 10,
                Type = "",
                Abrv = ""
            }, new VehicleOwner
            {
                Id = 0,
                FirstName = "S",
                LastName = "V",
                DOB = new DateTime(2000, 1, 1)
            });
        registration.Should().NotBeNull();
    }
}