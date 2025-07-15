using Mono.Model.Common;

namespace Mono.Model;

public class VehicleEngineType : IVehicleEngineType
{
    public required long Id { get; set; }
    public required string Type { get; set; }
    public required string Abrv { get; set; }

    public ICollection<VehicleRegistration> Registrations { get; set; } = new HashSet<VehicleRegistration>();
}