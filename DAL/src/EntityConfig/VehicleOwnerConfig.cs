using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mono.Model;

namespace Mono.DAL.EntityConfig;

public class VehicleOwnerConfig : IEntityTypeConfiguration<VehicleOwner>
{
    public void Configure(EntityTypeBuilder<VehicleOwner> builder)
    {
        builder.ToTable("VehicleOwners");

        builder.Property(cfg => cfg.Id).IsRequired();

        builder.Property(cfg => cfg.FirstName).IsRequired();
        builder.Property(cfg => cfg.LastName).IsRequired();
        builder.Property(cfg => cfg.DOB).IsRequired();
    }
}