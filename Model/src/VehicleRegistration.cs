namespace Mono.Model;

public class VehicleRegistration
{
    public int Id { get; set; }
    public required string RegistrationNumber { get; set; }
    
    // Foreign keys
    public int VehicleModelId { get; set; }
    public int VehicleEngineTypeId { get; set; }
    public int VehicleOwnerId { get; set; }
    
    // Navigation properties
    public VehicleModel Model { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public VehicleOwner Owner { get; set; }
}