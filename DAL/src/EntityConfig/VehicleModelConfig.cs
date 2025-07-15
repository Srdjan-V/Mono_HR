using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model;

namespace Mono.DAL.EntityConfig;

public class VehicleModelConfig : IEntityTypeConfiguration<VehicleModel>
{
    public void Configure(EntityTypeBuilder<VehicleModel> builder)
    {
        builder.ToTable("VehicleModels");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.Name).IsRequired();
        builder.Property(cfg => cfg.Abrv).IsRequired();

        builder.HasOne(cfg => cfg.Make)
            .WithMany(cfg => cfg.Models).HasForeignKey(cfg => cfg.VehicleMakeId);
    }
}