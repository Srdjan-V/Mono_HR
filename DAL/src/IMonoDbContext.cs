using Microsoft.EntityFrameworkCore;
using Mono.Model.Common;

namespace Mono.DAL;

public interface IMonoDbContext : IDisposable
{
    public int SaveChanges();

    public DbSet<IVehicleOwner> VehicleOwners();

    public DbSet<IVehicleMake> VehicleMakes();

    public DbSet<IVehicleModel> VehicleModels();

    public DbSet<IVehicleRegistration> Registrations();
}