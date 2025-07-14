using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Model;

[Table("VehicleOwners")]
public class VehicleOwner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VehicleOwnerId { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DOB { get; set; } // Date of Birth

    // Navigation property
    public ICollection<VehicleRegistration> Registrations { get; protected set; }

    public VehicleOwner()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}