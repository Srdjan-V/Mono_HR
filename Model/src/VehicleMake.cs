using Mono.Model.Common;

namespace Mono.Model;

public class VehicleMake : IVehicleMake
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property
    public ICollection<IVehicleModel> Models { get; protected set; }

    public VehicleMake()
    {
        Models = new HashSet<IVehicleModel>();
    }
}