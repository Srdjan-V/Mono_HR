using Mono.Model.Common;

namespace Mono.Model;

public class VehicleModel : IVehicleModel
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Foreign key
    public required long VehicleMakeId { get; set; }

    // Navigation properties
    public VehicleMake Make { get; set; }

    public ICollection<VehicleRegistration> Registrations { get; set; } = new HashSet<VehicleRegistration>();
}