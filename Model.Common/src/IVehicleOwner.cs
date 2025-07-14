namespace Mono.Model.Common;

public interface IVehicleOwner : IBaseEntity
{
    string FirstName { get; set; }
    string LastName { get; set; }
    DateTime DOB { get; set; } // Date of Birth
    ICollection<IVehicleRegistration> Registrations { get; }
}