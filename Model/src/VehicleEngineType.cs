namespace Mono.Model;

public class VehicleEngineType
{
    public required int Id { get; set; }
    public required string Type { get; set; }
    public required string Abrv { get; set; }

    public List<VehicleRegistration> Registrations { get; } = [];
}