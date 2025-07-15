using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model.Common;

namespace Mono.DAL.EntityConfig;

public class VehicleModelConfig : IEntityTypeConfiguration<IVehicleModel>
{
    public void Configure(EntityTypeBuilder<IVehicleModel> builder)
    {
        builder.ToTable("VehicleModels");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Name).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();

        builder.HasOne(cfg => cfg.Make)
            .WithMany(cfg => cfg.Models).HasForeignKey(cfg => cfg.VehicleMakeId);
    }
}