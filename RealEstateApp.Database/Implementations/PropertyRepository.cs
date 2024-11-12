using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RealEstateDbContext _context;

        public PropertyRepository(RealEstateDbContext context)
        {
            _context = context;
        }
        public async Task AddProperty(Property newProperty)
        {
            await _context.Properties.AddAsync(newProperty);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Property>> GetOwnedProperties(int ownerId)
        {
            return await _context.Properties.Include(p=>p.Owner).Include(p=>p.Location).Where(p=>p.OwnerId==ownerId).ToListAsync();
                                
        }
    }
}