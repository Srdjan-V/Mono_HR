using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model.Common;

namespace Mono.DAL.EntityConfig;

public class VehicleMakeConfig : IEntityTypeConfiguration<IVehicleMake>
{
    public void Configure(EntityTypeBuilder<IVehicleMake> builder)
    {
        builder.ToTable("VehicleMakes");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Name).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();
    }
}