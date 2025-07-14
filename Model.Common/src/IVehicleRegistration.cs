namespace Mono.Model.Common;

public interface IVehicleRegistration : IBaseEntity
{
    string RegistrationNumber { get; set; }
    int VehicleModelId { get; set; }
    int VehicleEngineTypeId { get; set; }
    int VehicleOwnerId { get; set; }
    IVehicleModel Model { get; set; }
    IVehicleEngineType EngineType { get; set; }
    IVehicleOwner Owner { get; set; }
}