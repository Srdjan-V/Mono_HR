using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Model;

[Table("VehicleEngineTypes")]
public class VehicleEngineType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required int VehicleEngineTypeId { get; set; }
    public required string Type { get; set; }
    public required string Abrv { get; set; }

    public ICollection<VehicleRegistration> Registrations { get; protected set; }
    
    public VehicleEngineType()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}