using Mono.Model.Common;

namespace Mono.Model;

public class VehicleRegistration : IVehicleRegistration
{
    public required long Id { get; set; }
    public required string RegistrationNumber { get; set; }

    // Foreign keys
    public long VehicleModelId { get; set; }
    public long VehicleEngineTypeId { get; set; }
    public long VehicleOwnerId { get; set; }

    // Navigation properties
    public VehicleModel Model { get; set; }

    IVehicleModel IVehicleRegistration.Model
    {
        get => Model;
        set => Model = (VehicleModel)value;
    }

    public VehicleEngineType EngineType { get; set; }

    IVehicleEngineType IVehicleRegistration.EngineType
    {
        get => EngineType;
        set => EngineType = (VehicleEngineType)value;
    }

    public VehicleOwner Owner { get; set; }

    IVehicleOwner IVehicleRegistration.Owner
    {
        get => Owner;
        set => Owner = (VehicleOwner)value;
    }
}