using Mono.Model.Common;

namespace Mono.Model;

public class VehicleRegistration : IVehicleRegistration
{
    public required long Id { get; set; }
    public required string RegistrationNumber { get; set; }

    // Foreign keys
    public required long VehicleModelId { get; set; }
    public required long VehicleEngineTypeId { get; set; }
    public required long VehicleOwnerId { get; set; }

    // Navigation properties
    public VehicleModel Model { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public VehicleOwner Owner { get; set; }
}