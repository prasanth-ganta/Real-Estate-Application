using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;
public class PropertySubTypeConfig : IEntityTypeConfiguration<PropertySubType>
{
    public void Configure(EntityTypeBuilder<PropertySubType> builder)
    {
        builder.HasData( new PropertySubType { Id = 1, Name = "BHK1"},
            new PropertySubType { Id = 2, Name = "BHK2" }, 
            new PropertySubType { Id = 3, Name = "BHK3" }, 
            new PropertySubType { Id = 4, Name = "BHK4" }, 
            new PropertySubType { Id = 5, Name = "Office" }, 
            new PropertySubType { Id = 6, Name = "Retail" }, 
            new PropertySubType { Id = 7, Name = "Industrial" }, 
            new PropertySubType { Id = 8, Name = "VacantLand" }, 
            new PropertySubType { Id = 9, Name = "AgricultureLand" }, 
            new PropertySubType { Id = 10, Name = "RecreationalLand" }, 
            new PropertySubType { Id = 11, Name = "Hotel" }, 
            new PropertySubType { Id = 12, Name = "Hospital" }, 
            new PropertySubType { Id = 13, Name = "School" },
            new PropertySubType { Id = 14, Name = "OldAgeHome" } 
        );    
    }
}