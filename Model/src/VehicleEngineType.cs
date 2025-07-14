using Mono.Model.Common;

namespace Mono.Model;

public class VehicleEngineType : IVehicleEngineType
{
    public required long Id { get; set; }
    public required string Type { get; set; }
    public required string Abrv { get; set; }

    public ICollection<IVehicleRegistration> Registrations { get; protected set; }

    public VehicleEngineType()
    {
        Registrations = new HashSet<IVehicleRegistration>();
    }
}