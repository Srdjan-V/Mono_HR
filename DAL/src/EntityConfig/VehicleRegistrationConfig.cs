using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model;

namespace Mono.DAL.EntityConfig;

public class VehicleRegistrationConfig : IEntityTypeConfiguration<VehicleRegistration>
{
    public void Configure(EntityTypeBuilder<VehicleRegistration> builder)
    {
        builder.ToTable("VehicleRegistrations");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.RegistrationNumber).IsRequired().HasMaxLength(20);

        builder.HasOne(cfg => cfg.Model)
            .WithMany(cfg => cfg.Registrations).HasForeignKey(cfg => cfg.VehicleModelId);

        builder.HasOne(cfg => cfg.EngineType)
            .WithMany(cfg => cfg.Registrations).HasForeignKey(cfg => cfg.VehicleEngineTypeId);

        builder.HasOne(cfg => cfg.Owner)
            .WithMany(cfg => cfg.Registrations).HasForeignKey(cfg => cfg.VehicleOwnerId);
    }
}