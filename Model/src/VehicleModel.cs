using Mono.Model.Common;

namespace Mono.Model;

public class VehicleModel : IVehicleModel
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Foreign key
    public long VehicleMakeId { get; set; }

    // Navigation properties
    public required VehicleMake Make { get; set; }

    IVehicleMake IVehicleModel.Make
    {
        get => Make;
        set => Make = (VehicleMake)value;
    }

    public ICollection<VehicleRegistration> Registrations { get; set; } = new HashSet<VehicleRegistration>();

    ICollection<IVehicleRegistration> IVehicleModel.Registrations
    {
        get => Registrations.Cast<IVehicleRegistration>().ToList();
        set => Registrations = value.Cast<VehicleRegistration>().ToList();
    }
}