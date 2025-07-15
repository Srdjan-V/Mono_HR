using Mono.Model.Common;

namespace Mono.Model;

public class VehicleMake : IVehicleMake
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property
    public ICollection<VehicleModel> Models { get; set; } = new HashSet<VehicleModel>();

    ICollection<IVehicleModel> IVehicleMake.Models
    {
        get => Models.Cast<IVehicleModel>().ToList();
        set => Models = value.Cast<VehicleModel>().ToList();
    }
}