using Mono.Model.Common;

namespace Mono.Model;

public class VehicleModel : IVehicleModel
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Foreign key
    public int VehicleMakeId { get; set; }

    // Navigation properties
    public required IVehicleMake Make { get; set; }
    public ICollection<IVehicleRegistration> Registrations { get; protected set; }


    public VehicleModel()
    {
        Registrations = new HashSet<IVehicleRegistration>();
    }
}