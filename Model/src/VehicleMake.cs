namespace Mono.Model;

public class VehicleMake
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property
    public ICollection<VehicleModel> Models { get; protected set; }
    
    public VehicleMake()
    {
        Models = new HashSet<VehicleModel>();
    }
}