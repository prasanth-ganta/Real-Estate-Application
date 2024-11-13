using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class DocumentConfig : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.Property(d => d.FileName)
            .IsRequired();

        builder.Property(d => d.Description)
            .IsRequired(false);
            
        builder.HasOne(d => d.Property)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.PropertyId);
    }
}
