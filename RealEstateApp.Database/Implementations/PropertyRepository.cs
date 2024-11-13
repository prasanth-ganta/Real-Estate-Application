using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Implementations
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RealEstateDbContext _context;

        public PropertyRepository(RealEstateDbContext context)
        {
            _context = context;
        }

        public async Task AddProperty(Property newProperty, string username)
        {
            await _context.Properties.AddAsync(newProperty);
            await _context.SaveChangesWithUserName(username);
        }

        public async Task<List<Property>> GetAllPendingProperties()
        {
            IQueryable<Property> query = LoadAllProperties()
                .Where(p => p.ApprovalStatusId == (int)ApprovalStatusEnum.Pending);

            return await query.ToListAsync();
        }

        public async Task<List<Property>> GetAllProperties(RetrivalOptionsEnum retivalOption)
        {         
            IQueryable<Property> query = LoadAllProperties()
                .Where(p => p.ApprovalStatusId == (int)ApprovalStatusEnum.Approved);

            if(retivalOption != RetrivalOptionsEnum.All){
                query = query.Where(p => p.PropertyStatusId == (int)retivalOption); 
            }

            return await query.ToListAsync();
        }

        public async Task<List<Property>> GetOwnedProperties(int ownerId, RetrivalOptionsEnum retivalOption)
        {
            IQueryable<Property> query = LoadAllProperties()
                .Where(p=>p.OwnerId==ownerId);
            
            if(retivalOption != RetrivalOptionsEnum.All){
                query = query.Where(p => p.PropertyStatusId == (int)retivalOption); 
            }

            return await query.ToListAsync(); 
        }

        public async Task<bool> SoftDeleteProperty(int id, string username)
        {
            Property property = await _context.Properties.FindAsync(id);

            if (property == null || !property.IsActive) return false;

            property.IsActive = false; 
            await _context.SaveChangesWithUserName(username);
            return true;
        }

        private IQueryable<Property> LoadAllProperties()
        {
            IQueryable<Property> query = _context.Properties
                .Include(p => p.Owner)
                .Include(p => p.ApprovalStatus)
                .Include(p => p.PropertyStatus)
                .Include(p => p.PropertyType)
                .Include(p => p.SubPropertyType)
                .Include(p => p.Documents)
                .Include(p => p.Location)
                .Where(p => p.IsActive == true);
            return query;
        }
    }
}