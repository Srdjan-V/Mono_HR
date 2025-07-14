using Mono.Model.Common;

namespace Mono.Model;

public class VehicleRegistration : IVehicleRegistration
{
    public required long Id { get; set; }
    public required string RegistrationNumber { get; set; }

    // Foreign keys
    public int VehicleModelId { get; set; }
    public int VehicleEngineTypeId { get; set; }
    public int VehicleOwnerId { get; set; }

    // Navigation properties
    public IVehicleModel Model { get; set; }
    public IVehicleEngineType EngineType { get; set; }
    public IVehicleOwner Owner { get; set; }
}