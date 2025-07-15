namespace Mono.Model.Common;

public interface IVehicleRegistration : IBaseEntity
{
    string RegistrationNumber { get; set; }
    long VehicleModelId { get; set; }
    long VehicleEngineTypeId { get; set; }
    long VehicleOwnerId { get; set; }
}