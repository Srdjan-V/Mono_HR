namespace Model.Common;

public class VehicleMake
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Abrv { get; set; }

    // Navigation property
    public List<VehicleModel> Models { get; } = [];
}