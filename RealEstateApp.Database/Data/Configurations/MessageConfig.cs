using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(m => m.Chat)
            .IsRequired();

        builder.HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverID)
            .OnDelete(DeleteBehavior.Restrict);    
    }
}
