using System.ComponentModel.DataAnnotations;

namespace Mono.WebAPI.dto;

public class VehicleOwnerCreateUpdateDto
{
    public long Id { get; set; }

    [Required] [StringLength(50)] public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required] public DateTime DOB { get; set; }
}