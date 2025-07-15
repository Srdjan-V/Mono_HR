using Mono.Model.Common;

namespace Mono.Service.Common;

public interface IVehicleService
{
    Task<IVehicleRegistration> RegisterVehicleAsync(
        IVehicleRegistration registrationRequest,
        IVehicleModel modelRequest,
        IVehicleMake makeRequest,
        IVehicleEngineType engineTypeRequest,
        IVehicleOwner ownerRequest
    );
}