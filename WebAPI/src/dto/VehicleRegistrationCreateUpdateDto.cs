using System.ComponentModel.DataAnnotations;

namespace Mono.WebAPI.dto;

public class VehicleRegistrationCreateUpdateDto
{
    public long Id { get; set; }

    [Required] [StringLength(20)] public string RegistrationNumber { get; set; }

    [Required] public long VehicleModelId { get; set; }

    [Required] public long VehicleEngineTypeId { get; set; }

    [Required] public long VehicleOwnerId { get; set; }
}