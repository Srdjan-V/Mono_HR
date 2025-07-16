using AutoMapper;
using Mono.DAL;
using Mono.Model;
using Mono.Repository;
using Mono.Repository.Common;
using Mono.Service;
using Mono.Service.Common;
using Mono.WebAPI.dto;
using Ninject.Activation.Providers;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Mono.WebAPI;

public class ServiceModule : NinjectModule
{
    public override void Load()
    {
        Bind<IRepositoryFactory<VehicleMake>>().ToFactory();
        Bind<IRepository<VehicleMake>>().To<MakeRepository>();

        Bind<IRepositoryFactory<VehicleModel>>().ToFactory();
        Bind<IRepository<VehicleModel>>().To<ModelRepository>();

        Bind<IRepositoryFactory<VehicleOwner>>().ToFactory();
        Bind<IRepository<VehicleOwner>>().To<OwnerRepository>();

        Bind<IRepositoryFactory<VehicleRegistration>>().ToFactory();
        Bind<IRepository<VehicleRegistration>>().To<RegistrationsRepository>();

        Bind<IRepositoryFactory<VehicleEngineType>>().ToFactory();
        Bind<IRepository<VehicleEngineType>>().To<EngineTypeRepository>();

        var mapperCfg = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<VehicleRegistrationDto, VehicleRegistration>();
            cfg.CreateMap<VehicleModelDto, VehicleModel>();
            cfg.CreateMap<VehicleMakeDto, VehicleMake>();
            cfg.CreateMap<VehicleEngineTypeDto, VehicleEngineType>();
            cfg.CreateMap<VehicleOwnerDto, VehicleOwner>();
        }, LoggerFactory.Create(builder => builder.AddConsole()));

        Bind<IMapper>().ToProvider(new ConstantProvider<IMapper>(mapperCfg.CreateMapper()));

        Bind<IMonoDbContextFactory>().ToFactory();
        //todo atm its an in memory 
        Bind<IMonoDbContext>().To<InMemorySqliteMonoDbContext>().InSingletonScope();

        Bind<IVehicleService>().To<VehicleService>();

        Bind<VehicleRegistrationController>().ToSelf();
    }
}