using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mono.Model;

[Table("VehicleRegistrations")]
public class VehicleRegistration
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required int VehicleRegistrationId { get; set; }

    public required string RegistrationNumber { get; set; }

    // Foreign keys
    public int VehicleModelId { get; set; }
    public int VehicleEngineTypeId { get; set; }
    public int VehicleOwnerId { get; set; }

    // Navigation properties
    public VehicleModel Model { get; set; }
    public VehicleEngineType EngineType { get; set; }
    public VehicleOwner Owner { get; set; }
}