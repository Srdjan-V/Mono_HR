using Mono.Model;
using Mono.Repository.Common;
using Mono.Service.Common;

namespace Mono.Service;

public class EngineTypeService : IEngineTypeService
{
    public async void Initialize(IRepositoryFactory<VehicleEngineType> factory)
    {
        using var repository = factory.Build();
        await repository.AddAsync(new VehicleEngineType
        {
            Id = 1,
            Type = "Electric",
            Abrv = "ELC"
        });

        await repository.AddAsync(new VehicleEngineType
        {
            Id = 2,
            Type = "Gasoline",
            Abrv = "GAS"
        });

        await repository.AddAsync(new VehicleEngineType
        {
            Id = 3,
            Type = "Diesel",
            Abrv = "DIS"
        });

        await repository.CommitAsync();
    }
}