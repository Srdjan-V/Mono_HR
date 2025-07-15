namespace Mono.Model.Common;

public interface IVehicleModel : IBaseEntity
{
    string Name { get; set; }
    string Abrv { get; set; }
    long VehicleMakeId { get; set; }
}