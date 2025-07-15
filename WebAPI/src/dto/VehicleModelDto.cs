namespace Mono.WebAPI.dto;

public class VehicleModelDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Abrv { get; set; }
    public VehicleMakeDto Make { get; set; }
}