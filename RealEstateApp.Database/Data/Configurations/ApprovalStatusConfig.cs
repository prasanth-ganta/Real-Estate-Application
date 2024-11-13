using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Data.Configurations;

public class ApprovalStatusConfig : IEntityTypeConfiguration<ApprovalStatus>
{
    public void Configure(EntityTypeBuilder<ApprovalStatus> builder)
    {
        builder.HasData(
            new ApprovalStatus { ID = 1, Status = "Pending" },
            new ApprovalStatus { ID = 2, Status = "Approved" }
        );    
    }
}
