using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model;

namespace Mono.DAL.EntityConfig;

public class VehicleMakeConfig : IEntityTypeConfiguration<VehicleMake>
{
    public void Configure(EntityTypeBuilder<VehicleMake> builder)
    {
        builder.ToTable("VehicleMakes");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Name).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();
    }
}