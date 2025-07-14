using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Model;

public class VehicleMake
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required int VehicleMakeId { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property
    public ICollection<VehicleModel> Models { get; protected set; }
    
    public VehicleMake()
    {
        Models = new HashSet<VehicleModel>();
    }
}