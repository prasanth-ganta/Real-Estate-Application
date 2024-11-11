using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class LocationConfig : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(l => l.City)
            .IsRequired();

        builder.Property(l => l.State)
            .IsRequired();

        builder.Property(l => l.Country)
            .IsRequired();

        builder.HasOne(l => l.Property)
            .WithOne(p => p.Location)
            .HasForeignKey<Property>(p => p.LocationId);
    }
}
