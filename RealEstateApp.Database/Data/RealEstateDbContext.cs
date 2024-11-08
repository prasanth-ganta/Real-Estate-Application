using Microsoft.EntityFrameworkCore;

namespace RealEstateApp.Database.Data;

public class RealEstateDbContext : DbContext
{
    public RealEstateDbContext(DbContextOptions options) : base(options)
    {
        
    }

}
