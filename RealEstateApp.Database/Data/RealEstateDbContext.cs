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
            modelBuilder.Entity(entity.Name).Property<DateTime?>("CreatedAt").HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity(entity.Name).Property<DateTime?>("ModifiedAt");
            modelBuilder.Entity(entity.Name).Property<string>("CreatedBy");
            modelBuilder.Entity(entity.Name).Property<string>("ModifiedBy");
        }

        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new ApprovalStatusConfig());
        modelBuilder.ApplyConfiguration(new PropertyStatusConfig());
        modelBuilder.ApplyConfiguration(new PropertySubTypeConfig());
        modelBuilder.ApplyConfiguration(new PropertyTypeConfig());
        modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfiguration(new PropertyConfig());
        modelBuilder.ApplyConfiguration(new DocumentConfig());
        modelBuilder.ApplyConfiguration(new LocationConfig());
        modelBuilder.ApplyConfiguration(new MessageConfig());
    }
    public async Task<int> SaveChangesWithUserName(string username, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                entry.Property("CreatedBy").CurrentValue = username;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("ModifiedAt").CurrentValue = DateTime.UtcNow;
                entry.Property("ModifiedBy").CurrentValue = username;
            }
        }
        return await SaveChangesAsync(cancellationToken);
    }
}
