using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;
public class PropertySubTypeConfig : IEntityTypeConfiguration<PropertySubType>
{
    public void Configure(EntityTypeBuilder<PropertySubType> builder)
    {
        builder.HasData( new PropertySubType { ID = 1, Name = "BHK1"},
            new PropertySubType { ID = 2, Name = "BHK2" }, 
            new PropertySubType { ID = 3, Name = "BHK3" }, 
            new PropertySubType { ID = 4, Name = "BHK4" }, 
            new PropertySubType { ID = 5, Name = "Office" }, 
            new PropertySubType { ID = 6, Name = "Retail" }, 
            new PropertySubType { ID = 7, Name = "Industrial" }, 
            new PropertySubType { ID = 8, Name = "VacantLand" }, 
            new PropertySubType { ID = 9, Name = "AgricultureLand" }, 
            new PropertySubType { ID = 10, Name = "RecreationalLand" }, 
            new PropertySubType { ID = 11, Name = "Hotel" }, 
            new PropertySubType { ID = 12, Name = "Hospital" }, 
            new PropertySubType { ID = 13, Name = "School" },
            new PropertySubType { ID = 14, Name = "OldAgeHome" } 
        );    
    }
}