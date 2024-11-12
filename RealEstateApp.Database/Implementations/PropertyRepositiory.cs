using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
public class PropertyRepository : IPropertyRepository
{
    private readonly RealEstateDbContext _realEstateDbContext;

    public PropertyRepository(RealEstateDbContext realEstateDbContext)
    {
        _realEstateDbContext = realEstateDbContext;
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


    // Get all Properties
    public async Task<IEnumerable<Property>> GetAllProperties(int userId)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.OwnerId != userId && p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

   //Get user owned properties
    public async Task<IEnumerable<Property>> GetOwnedPropertiesByUser (int userId)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.OwnerId == userId && p.ApprovalStatus.Status == "Approved")
            .ToListAsync();
    }

    // Get favorite properties for a specific user
    public async Task<IEnumerable<Property>> GetFavoritePropertiesByUser (int userId)
    {
        return await _realEstateDbContext.Properties
            .Include(p => p.Location)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.PropertyStatus)
            .Where(p => p.FavouritedByUsers.Any(u => u.ID == userId))
            .ToListAsync();
    }

    public async Task<bool> ChangePropertyStatusToSell(int propertyId)
    {
        Property? property = await _realEstateDbContext.Properties
            .Include(p => p.PropertyStatus)
            .FirstOrDefaultAsync(p => p.ID == propertyId);

        if (property == null || property.PropertyStatus.Status == "Sell")
        {
            return false;
        }

        property.PropertyStatus.Status = "Sell";

        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

}
