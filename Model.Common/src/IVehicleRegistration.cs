namespace Mono.Model.Common;

public interface IVehicleRegistration : IBaseEntity
{
    string RegistrationNumber { get; set; }
    long VehicleModelId { get; set; }
    long VehicleEngineTypeId { get; set; }
    long VehicleOwnerId { get; set; }
    IVehicleModel Model { get; set; }
    IVehicleEngineType EngineType { get; set; }
    IVehicleOwner Owner { get; set; }
}