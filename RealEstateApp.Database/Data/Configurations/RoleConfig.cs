using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Seed data
        Role userRole = new Role { ID = 1, Name = "User" };
        Role adminRole = new Role { ID = 2, Name = "Admin" };
        builder.HasData(userRole, adminRole);

    }
}
