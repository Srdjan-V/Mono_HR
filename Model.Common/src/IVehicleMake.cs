namespace Mono.Model.Common;

public interface IVehicleMake : IBaseEntity
{
    string Name { get; set; }
    string Abrv { get; set; }
    ICollection<IVehicleModel> Models { get; }
}