using Mono.Model.Common;

namespace Mono.Model;

public class VehicleOwner : IVehicleOwner
{
    public required long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DOB { get; set; } // Date of Birth

    // Navigation property
    public ICollection<VehicleRegistration> Registrations { get; set; } = new HashSet<VehicleRegistration>();

    ICollection<IVehicleRegistration> IVehicleOwner.Registrations
    {
        get => Registrations.Cast<IVehicleRegistration>().ToList();
        set => Registrations = value.Cast<VehicleRegistration>().ToList();
    }
}