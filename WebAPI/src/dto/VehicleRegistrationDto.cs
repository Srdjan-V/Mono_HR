namespace Mono.WebAPI.dto;

public class VehicleRegistrationDto
{
    public int Id { get; set; }
    public string RegistrationNumber { get; set; }
    public VehicleModelDto Model { get; set; }
    public VehicleEngineTypeDto EngineType { get; set; }
    public VehicleOwnerDto Owner { get; set; }
}