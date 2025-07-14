using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model.Common;

namespace Mono.DAL.EntityConfig;

public class VehicleEngineTypeConfig : IEntityTypeConfiguration<IVehicleEngineType>
{
    public void Configure(EntityTypeBuilder<IVehicleEngineType> builder)
    {
        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Type).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();
    }
}