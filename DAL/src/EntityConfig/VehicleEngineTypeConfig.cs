using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model;

namespace Mono.DAL.EntityConfig;

public class VehicleEngineTypeConfig : IEntityTypeConfiguration<VehicleEngineType>
{
    public void Configure(EntityTypeBuilder<VehicleEngineType> builder)
    {
        builder.ToTable("VehicleEngineTypes");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Type).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();
    }
}