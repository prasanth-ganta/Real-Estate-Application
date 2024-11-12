using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data.Configurations;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
    public DbSet<PropertyStatus> PropertyStatuses { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<PropertySubType> PropertySubTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure shadow properties to all entities
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entity.Name).Property<DateTime>("CreatedAt").HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity(entity.Name).Property<DateTime?>("ModifiedAt").HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity(entity.Name).Property<string>("CreatedBy");
            modelBuilder.Entity(entity.Name).Property<string>("ModifiedBy");
            modelBuilder.Entity(entity.Name).Property<bool>("IsActive").HasDefaultValue(true);
        }

        // Seed data
        modelBuilder.Entity<Role>().HasData(
            new Role { ID = 1, Name = "User" },
            new Role { ID = 2, Name = "Admin" }
        );

        modelBuilder.Entity<ApprovalStatus>().HasData(
            new ApprovalStatus { ID = 1, Status = "Pending" },
            new ApprovalStatus { ID = 2, Status = "Approved" }
        );

        modelBuilder.Entity<PropertyStatus>().HasData(
            new PropertyStatus { ID = 1, Status = "Rent" },
            new PropertyStatus { ID = 2, Status = "Sell" },
            new PropertyStatus { ID = 3, Status = "Unavailable" }
        );
        
        modelBuilder.Entity<PropertyType>().HasData(
            new PropertyType { ID=1, Name = "Residential" },
            new PropertyType { ID=2, Name = "Commercial" },
            new PropertyType { ID=3, Name = "Land" },
            new PropertyType { ID=4, Name = "SpecialPurpose" },
            new PropertyType { ID=5, Name = "Luxuary" }
        );

        modelBuilder.Entity<PropertySubType>().HasData(
            new PropertySubType { ID=1, Name = "BHK1" },
            new PropertySubType { ID=2, Name = "BHK2" },
            new PropertySubType { ID=3, Name = "BHK3" },
            new PropertySubType { ID=4, Name = "BHK4" },
            new PropertySubType { ID=5, Name = "Office" },
            new PropertySubType { ID=6, Name = "Retail" },
            new PropertySubType { ID=7, Name = "Industrial" },
            new PropertySubType { ID=8, Name = "VacantLand" },
            new PropertySubType { ID=9, Name = "AgricultureLand" },
            new PropertySubType { ID=10, Name = "RecreationalLand" },
            new PropertySubType { ID=11, Name = "Hotel" },
            new PropertySubType { ID=12, Name = "Hospital" },
            new PropertySubType { ID=13, Name = "School" },
            new PropertySubType { ID=14, Name = "OldAgeHome" }
        ); 

        modelBuilder.ApplyConfiguration( new UserConfig());
        modelBuilder.ApplyConfiguration( new PropertyConfig());
        modelBuilder.ApplyConfiguration( new DocumentConfig());
        modelBuilder.ApplyConfiguration( new LocationConfig());
        modelBuilder.ApplyConfiguration( new MessageConfig());
    }
}
