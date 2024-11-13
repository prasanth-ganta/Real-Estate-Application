using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Implementations;
public class PropertyRepository : IPropertyRepository
{

    private readonly RealEstateDbContext _realEstateDbContext;

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
    public PropertyRepository(RealEstateDbContext realEstateDbContext)
    {
        _realEstateDbContext = realEstateDbContext;
    }
    public async Task AddProperty(Property newProperty)
    {
        await _realEstateDbContext.Properties.AddAsync(newProperty);
        await _realEstateDbContext.SaveChangesAsync();
    }
    public async Task<List<Property>> GetOwnedProperties(int ownerId)
    {
        return await _realEstateDbContext.Properties.Include(p => p.Owner).Include(p => p.Location).Where(p => p.OwnerId == ownerId).ToListAsync();

    }

    public async Task UpdatePropertyStatus(Property updatedProperty)
    {
        _realEstateDbContext.Properties.Update(updatedProperty);
        await _realEstateDbContext.SaveChangesAsync();
    }

    //// Buy Methods Implementation
    public async Task<IEnumerable<Property>> GetPropertiesForBuyByLocation(string city, string state)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Sell" &&
                   p.Location.City.Contains(city) &&
                   p.Location.State.Contains(state) &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesForBuyByPincode(int zipCode)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Sell" &&
                   p.Location.ZipCode == zipCode &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesForBuyByPriceRange(double minPrice, double maxPrice)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Sell" &&
                   p.Price >= minPrice &&
                   p.Price <= maxPrice &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    //// Rent Methods Implementation
    public async Task<IEnumerable<Property>> GetPropertiesForRentByLocation(string city, string state)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Rent" &&
                   p.Location.City.Contains(city) &&
                   p.Location.State.Contains(state) &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesForRentByPincode(int zipCode)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Rent" &&
                   p.Location.ZipCode == zipCode &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesForRentByPriceRange(double minPrice, double maxPrice)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Rent" &&
                   p.Price >= minPrice &&
                   p.Price <= maxPrice &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesForRentByName(string propertyName)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.PropertyStatus.Status == "Rent" &&
                   p.Name.Contains(propertyName) &&
                   p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    public async Task<bool> AddDocument(Document document,int propertyId){
        document.PropertyId = propertyId;
        _realEstateDbContext.Documents.AddAsync(document);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> DeleteDocument(int documentId, int propertyId)
    {
        var document = await _realEstateDbContext.Documents
            .FirstOrDefaultAsync(d => d.ID == documentId && d.PropertyId == propertyId);
        
        if (document == null)
        {
            return false;
        }
        _realEstateDbContext.Documents.Remove(document);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

    //Favourite 
    public async Task<bool> AddToFavorites(int userId, int propertyId)
    {
        var user = await _realEstateDbContext.Users
            .Include(u => u.FavouriteProperties)
            .FirstOrDefaultAsync(u => u.ID == userId);

        var property = await _realEstateDbContext.Properties
            .FirstOrDefaultAsync(p => p.ID == propertyId);

        if (user == null || property == null)
        {
            return false;
        }

        if (user.FavouriteProperties.Any(p => p.ID == propertyId))
        {
            return false;
        }

        user.FavouriteProperties.Add(property);
        await _realEstateDbContext.SaveChangesAsync();
        return true;

    }
    

    public async Task<bool> RemoveFromFavorites(int userId, int propertyId)
    {
        var user = await _realEstateDbContext.Users
            .Include(u => u.FavouriteProperties)
            .FirstOrDefaultAsync(u => u.ID == userId);

        if (user == null)
        {
            return false;
        }

        var property = user.FavouriteProperties
            .FirstOrDefault(p => p.ID == propertyId);

        if (property == null)
        {
            return false;
        }

        user.FavouriteProperties.Remove(property);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

    
}