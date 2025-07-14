namespace Mono.Model;

public class VehicleEngineType
{
    public required int Id { get; set; }
    public required string Type { get; set; }
    public required string Abrv { get; set; }

    public ICollection<VehicleRegistration> Registrations { get; protected set; }
    
    public VehicleEngineType()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}