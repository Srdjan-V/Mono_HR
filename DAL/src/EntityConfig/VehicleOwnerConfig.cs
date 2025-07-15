using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model.Common;

namespace Mono.DAL.EntityConfig;

public class VehicleOwnerConfig : IEntityTypeConfiguration<IVehicleOwner>
{
    public void Configure(EntityTypeBuilder<IVehicleOwner> builder)
    {
        builder.ToTable("VehicleOwners");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.FirstName).IsRequired();
        builder.Property(cfg => cfg.LastName).IsRequired();
        builder.Property(cfg => cfg.DOB).IsRequired();
    }
}