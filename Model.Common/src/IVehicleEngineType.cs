namespace Mono.Model.Common;

public interface IVehicleEngineType : IBaseEntity
{
    string Type { get; set; }
    string Abrv { get; set; }
    ICollection<IVehicleRegistration> Registrations { get; }
}