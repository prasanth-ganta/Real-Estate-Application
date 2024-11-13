using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;


namespace RealEstateApp.Database.Data.Configurations;

public class PropertyConfig : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.PropertyType)
            .WithMany(pt => pt.Properties)
            .HasForeignKey(p => p.PropertyTypeID);

        builder.HasOne(p => p.SubPropertyType)
            .WithMany(spt => spt.Properties)
            .HasForeignKey(p => p.SubPropertyTypeID);

        builder.HasOne(p => p.Owner)
            .WithMany(u => u.OwnedProperties)
            .HasForeignKey(p => p.OwnerID);

        builder.HasOne(p => p.ApprovalStatus)
            .WithMany(a => a.Properties)
            .HasForeignKey(p => p.ApprovalStatusID);

        builder.HasOne(p => p.PropertyStatus)
            .WithMany(ps => ps.Properties)
            .HasForeignKey(p => p.PropertyStatusID);   
        
        builder.HasMany(p => p.FavouritedByUsers)
            .WithMany(u => u.FavouriteProperties)
            .UsingEntity
            ("Favourites",
                l => l.HasOne(typeof(User)).WithMany().OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne(typeof(Property)).WithMany().OnDelete(DeleteBehavior.Restrict)
            );
    }
}
