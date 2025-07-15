using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mono.Model.Common;

namespace Mono.DAL;

public abstract class DefaultMonoDbContext : DbContext, IMonoDbContext
{
    protected DefaultMonoDbContext()
    {
    }

    protected DefaultMonoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType is { IsGenericType: true } &&
                           type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
        foreach (var type in typesToRegister)
        {
            dynamic? configurationInstance = Activator.CreateInstance(type);
            if (configurationInstance == null)
            {
                throw new ArgumentException("Configuration instance is null");
            }

            modelBuilder.ApplyConfiguration(configurationInstance);
        }

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<IVehicleOwner> VehicleOwners()
    {
        return this.Set<IVehicleOwner>();
    }

    public DbSet<IVehicleMake> VehicleMakes()
    {
        return this.Set<IVehicleMake>();
    }

    public DbSet<IVehicleModel> VehicleModels()
    {
        return this.Set<IVehicleModel>();
    }

    public DbSet<IVehicleRegistration> Registrations()
    {
        return this.Set<IVehicleRegistration>();
    }
}