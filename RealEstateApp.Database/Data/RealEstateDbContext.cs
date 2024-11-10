using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions options) : base(options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<FavouriteProperty> FavouriteProperties { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Many-to-many relationship between User and Role
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("User  Roles"));

        // One-to-many relationship between User and Property
        modelBuilder.Entity<Property>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Properties)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Cascade); // If a User is deleted, their Properties are deleted

        // One-to-many relationship between Property and Document
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Property)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.PropertyId)
            .OnDelete(DeleteBehavior.Cascade); // If a Property is deleted, its Documents are deleted

        // One-to-many relationship between User and Message (Sent)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict); // Messages should not be deleted when a User is deleted
                
        // One-to-many relationship between User and Message (Received)
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict); // Messages should not be deleted when a User is deleted

        // One-to-many relationship between Property and FavouriteProperty
        modelBuilder.Entity<FavouriteProperty>()
            .HasOne(fp => fp.Property)
            .WithMany(p => p.FavouriteByUsers)
            .HasForeignKey(fp => fp.PropertyId)
            .OnDelete(DeleteBehavior.Cascade); // If a Property is deleted, its FavouriteProperties are deleted

        // One-to-many relationship between User and FavouriteProperty
        modelBuilder.Entity<FavouriteProperty>()
            .HasOne(fp => fp.User)
            .WithMany(u => u.FavouriteProperties)
            .HasForeignKey(fp => fp.UserId)
            .OnDelete(DeleteBehavior.Cascade); // If a User is deleted, their FavouriteProperties are deleted

        // One-to-one relationship between Location and Property
        modelBuilder.Entity<Property>()
            .HasOne(p => p.Location)
            .WithOne(l => l.Property)
            .HasForeignKey<Property>(p => p.LocationId)
            .OnDelete(DeleteBehavior.Cascade); // If a Property is deleted, its Location is deleted
    }

}
