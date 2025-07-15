using Microsoft.EntityFrameworkCore;
using Mono.Model;

namespace Mono.DAL;

public interface IMonoDbContext : IDisposable
{
    public Task<int> SaveChangesAsync();

    public DbSet<VehicleEngineType> EngineTypes();

    public DbSet<VehicleOwner> VehicleOwners();

    public DbSet<VehicleMake> VehicleMakes();

    public DbSet<VehicleModel> VehicleModels();

    public DbSet<VehicleRegistration> Registrations();
}