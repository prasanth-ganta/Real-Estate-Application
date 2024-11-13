using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class PropertyTypeConfig : IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.HasData(
            new PropertyType { ID = 1, Name = "Residential" }, 
            new PropertyType { ID = 2, Name = "Commercial" }, 
            new PropertyType { ID = 3, Name = "Land" }, 
            new PropertyType { ID = 4,Name = "Special Purpose" }, 
            new PropertyType { ID = 5, Name = "Luxury" }
        );
    }
}
