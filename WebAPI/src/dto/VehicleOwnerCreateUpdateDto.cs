using System.ComponentModel.DataAnnotations;

namespace Mono.WebAPI.dto;

public class VehicleOwnerCreateUpdateDto
{
    [Required] [StringLength(50)] public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required] public DateTime DOB { get; set; }
}