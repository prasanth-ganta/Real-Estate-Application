using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class PropertyStatusConfig : IEntityTypeConfiguration<PropertyStatus>
{
    public void Configure(EntityTypeBuilder<PropertyStatus> builder)
    {
        builder.HasData(
            new PropertyStatus { ID = 1, Status = "Rent" },
            new PropertyStatus { ID = 2, Status = "Sell"},
            new PropertyStatus { ID = 3, Status = "Unavailable" }
        );    
    }
}
