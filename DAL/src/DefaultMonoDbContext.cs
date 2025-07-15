using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mono.Model;

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
            .Where(type => type is { Namespace: not null, IsClass: true, IsAbstract: false })
            .Where(type =>
                type.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)) ||
                (type.BaseType != null &&
                 type.BaseType.IsGenericType &&
                 type.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
            );
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

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    public DbSet<VehicleEngineType> EngineTypes()
    {
        return this.Set<VehicleEngineType>();
    }

    public DbSet<VehicleOwner> VehicleOwners()
    {
        return this.Set<VehicleOwner>();
    }

    public DbSet<VehicleMake> VehicleMakes()
    {
        return this.Set<VehicleMake>();
    }

    public DbSet<VehicleModel> VehicleModels()
    {
        return this.Set<VehicleModel>();
    }

    public DbSet<VehicleRegistration> Registrations()
    {
        return this.Set<VehicleRegistration>();
    }
}