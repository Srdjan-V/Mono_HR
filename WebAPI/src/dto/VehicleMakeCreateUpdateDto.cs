using System.ComponentModel.DataAnnotations;

namespace Mono.WebAPI.dto;

public class VehicleMakeCreateUpdateDto
{
    [Required] [StringLength(50)] public string Name { get; set; }

    [Required] [StringLength(10)] public string Abrv { get; set; }
}