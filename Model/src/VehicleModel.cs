using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Model;

[Table("VehicleModels")]
public class VehicleModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int VehicleModelId { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }
    
    // Foreign key
    public int VehicleMakeId { get; set; }

    // Navigation properties
    public required VehicleMake Make { get; set; }
    public ICollection<VehicleRegistration> Registrations { get; protected set; }


    public VehicleModel()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}