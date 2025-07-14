namespace Mono.Model;

public class VehicleOwner
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DOB { get; set; }  // Date of Birth
    
    // Navigation property
    public ICollection<VehicleRegistration> Registrations { get; protected set; }

    public VehicleOwner()
    {
        Registrations = new HashSet<VehicleRegistration>();
    }
}