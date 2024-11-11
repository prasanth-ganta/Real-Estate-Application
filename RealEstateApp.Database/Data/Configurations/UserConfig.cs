using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.Property(u => u.LastName)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();
        
        builder.Property(u => u.UserName)
            .IsRequired();

        builder.Property(u => u.Password)
            .IsRequired();    
            
        var users = new List<User>
        {
            new User 
            { 
                ID = 1,
                FirstName = "Abdul", 
                LastName = "Shaik", 
                Email = "abdul@example.com", 
                UserName = "abdul", 
                Password = BCrypt.Net.BCrypt.HashPassword("Abdul@123")
            },
            new User 
            { 
                ID = 2,
                FirstName = "Prashanth", 
                LastName = "Ganta", 
                Email = "prashanth@example.com", 
                UserName = "prashanth", 
                Password = BCrypt.Net.BCrypt.HashPassword("Prashanth@123")
            }
        };
        builder.HasData(users);
        builder.HasMany(r => r.Roles)
            .WithMany(u => u.Users)
            .UsingEntity<Dictionary<string, object>>(
                "User Roles",
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasPrincipalKey("ID"),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasPrincipalKey("ID"),
                j =>
                {
                    j.HasKey("UserId", "RoleId");
                    j.HasData(
                        new { UserId = 1, RoleId = 1 }, 
                        new { UserId = 1, RoleId = 2 }, 
                        new { UserId = 2, RoleId = 1 }, 
                        new { UserId = 2, RoleId = 2 }  
                    );
                });

    }
}
