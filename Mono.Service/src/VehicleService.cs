using Mono.Model;
using Mono.Model.Common;
using Mono.Repository.Common;
using Mono.Service.Common;

namespace Mono.Service;

public class VehicleService(
    IRepositoryFactory<VehicleMake> makeFactory,
    IRepositoryFactory<VehicleModel> modelFactory,
    IRepositoryFactory<VehicleOwner> ownerFactory,
    IRepositoryFactory<VehicleRegistration> registrationFactory,
    IRepositoryFactory<VehicleEngineType> engineTypeFactory
) : IVehicleService
{
    public async Task<IVehicleRegistration> RegisterVehicleAsync(
        IVehicleRegistration registrationRequest,
        IVehicleModel modelRequest,
        IVehicleMake makeRequest,
        IVehicleEngineType engineTypeRequest,
        IVehicleOwner ownerRequest
    )
    {
        ArgumentNullException.ThrowIfNull(registrationRequest);
        ArgumentNullException.ThrowIfNull(modelRequest);
        ArgumentNullException.ThrowIfNull(makeRequest);
        ArgumentNullException.ThrowIfNull(engineTypeRequest);
        ArgumentNullException.ThrowIfNull(ownerRequest);

        var owner = await RegisterOrGetOwnerAsync(ownerRequest);

        var engineType = await GetEngineTypeAsync(engineTypeRequest.Id);
        var make = await RegisterOrGetMakeAsync(makeRequest);
        var model = await RegisterOrGetModelAsync(modelRequest, make);

        using var repository = registrationFactory.Build();
        var data = new VehicleRegistration
        {
            Id = registrationRequest.Id,
            RegistrationNumber = registrationRequest.RegistrationNumber,
            VehicleModelId = model.Id,
            VehicleEngineTypeId = engineType.Id,
            VehicleOwnerId = owner.Id,
        };

        await repository.AddAsync(data);
        await repository.CommitAsync();
        return data;
    }


    //todo ad get or by abrv/name?, add methods to repos?
    private async Task<VehicleModel> RegisterOrGetModelAsync(IVehicleModel request, VehicleMake makeRequest)
    {
        using var repository = modelFactory.Build();
        var dbModel = await repository.GetAsync(request.Id);
        if (dbModel != null)
        {
            return dbModel;
        }

        VehicleModel data = new VehicleModel
        {
            Id = request.Id,
            Name = request.Name,
            Abrv = request.Abrv,
            VehicleMakeId = makeRequest.Id,
        };

        await repository.AddAsync(data);

        await repository.CommitAsync();
        return data;
    }

    private async Task<VehicleMake> RegisterOrGetMakeAsync(IVehicleMake request)
    {
        using var repository = makeFactory.Build();
        var dbModel = await repository.GetAsync(request.Id);
        if (dbModel != null)
        {
            return dbModel;
        }

        VehicleMake data = new VehicleMake
        {
            Id = request.Id,
            Name = request.Name,
            Abrv = request.Abrv,
        };
        await repository.AddAsync(data);
        await repository.CommitAsync();
        return data;
    }

    private async Task<IVehicleEngineType> GetEngineTypeAsync(long id)
    {
        using var repository = engineTypeFactory.Build();

        VehicleEngineType? dbModel = await repository.GetAsync(id);
        if (dbModel == null)
        {
            throw new ArgumentException("Vehicle engine type not found");
        }

        return dbModel;
    }

    private async Task<VehicleOwner> RegisterOrGetOwnerAsync(IVehicleOwner request)
    {
        using var repository = ownerFactory.Build();

        var dbModel = await repository.GetAsync(request.Id);
        if (dbModel != null)
        {
            return dbModel;
        }

        var data = new VehicleOwner
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DOB = request.DOB
        };
        await repository.AddAsync(data);
        await repository.CommitAsync();
        return data;
    }
}