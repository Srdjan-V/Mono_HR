namespace Mono.Model;

public class VehicleModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }
    
    // Foreign key
    public int VehicleMakeId { get; set; }
    
    // Navigation properties
    public required VehicleMake Make { get; set; }
    public ICollection<VehicleRegistration> Registrations { get; protected set; }


    public VehicleModel()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}