using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Implementations;
public class PropertyRepository : IPropertyRepository
{

    private readonly RealEstateDbContext _realEstateDbContext;
    public PropertyRepository(RealEstateDbContext realEstateDbContext)
    {
        _realEstateDbContext = realEstateDbContext;
    }

    public async Task<bool> AddProperty(Property newProperty, string username)
    {
        try
        {
            await _realEstateDbContext.Properties.AddAsync(newProperty);
            await _realEstateDbContext.SaveChangesWithUserName(username);
            return true;
        }
        catch(Exception exception)
        {
            throw;
        }
    }

    public async Task<List<Property>> GetAllPendingProperties()
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.ApprovalStatusID == (int)ApprovalStatusEnum.Pending);

        return await propertyList.ToListAsync();
    }

    public async Task<bool> ApproveProperty(int propertyID)
    {
        Property property = await LoadAllProperties().FirstOrDefaultAsync(p => p.ID == propertyID);
            
        if(property == null || property.ApprovalStatusID == (int)ApprovalStatusEnum.Approved)
        {
            return false;
        }
        else
        {
            property.ApprovalStatusID = (int)ApprovalStatusEnum.Approved;
            await _realEstateDbContext.SaveChangesAsync();
            return true;
        }
    }

    public async Task<List<Property>> GetAllProperties(PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.ApprovalStatusID == (int)ApprovalStatusEnum.Approved);

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();
    }

    public async Task<List<Property>> GetOwnedProperties(int ownerID, PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.OwnerID == ownerID);

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();
    }

    public async Task<bool> SoftDeleteProperty(int id, string username)
    {
        Property property = await _realEstateDbContext.Properties.FindAsync(id);

        if (property == null || !property.IsActive) return false;

        property.IsActive = false;
        await _realEstateDbContext.SaveChangesWithUserName(username);
        return true;
    }

    private IQueryable<Property> LoadAllProperties()
    {
        IQueryable<Property> propertyList = _realEstateDbContext.Properties
            .Include(p => p.Owner)
            .Include(p => p.ApprovalStatus)
            .Include(p => p.PropertyStatus)
            .Include(p => p.PropertyType)
            .Include(p => p.SubPropertyType)
            .Include(p => p.Documents)
            .Include(p => p.Location)
            .Where(p => p.IsActive == true);
        return propertyList;
    }
    public async Task<List<Property>> GetOwnedProperties(int ownerID)
    {
        return await _realEstateDbContext.Properties.Include(p => p.Owner).Include(p => p.Location).Where(p => p.OwnerID == ownerID).ToListAsync();

    }

    public async Task UpdatePropertyStatus(Property updatedProperty)
    {
        _realEstateDbContext.Properties.Update(updatedProperty);
        await _realEstateDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteDocument(int documentID, int propertyID)
    {
        var document = await _realEstateDbContext.Documents
            .FirstOrDefaultAsync(d => d.ID == documentID && d.PropertyID == propertyID);

        if (document == null)
        {
            return false;
        }
        _realEstateDbContext.Documents.Remove(document);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

    //Favourite 
    public async Task<bool> AddToFavorites(int userID, int propertyID)
    {
        var user = await _realEstateDbContext.Users
            .Include(u => u.FavouriteProperties)
            .FirstOrDefaultAsync(u => u.ID == userID);

        var property = await _realEstateDbContext.Properties
            .FirstOrDefaultAsync(p => p.ID == propertyID);

        if (user == null || property == null)
        {
            return false;
        }

        if (user.FavouriteProperties.Any(p => p.ID == propertyID))
        {
            return false;
        }

        user.FavouriteProperties.Add(property);
        await _realEstateDbContext.SaveChangesAsync();
        return true;

    }


    public async Task<bool> RemoveFromFavorites(int userID, int propertyID)
    {
        var user = await _realEstateDbContext.Users
            .Include(u => u.FavouriteProperties)
            .FirstOrDefaultAsync(u => u.ID == userID);

        if (user == null)
        {
            return false;
        }

        var property = user.FavouriteProperties
            .FirstOrDefault(p => p.ID == propertyID);

        if (property == null)
        {
            return false;
        }

        user.FavouriteProperties.Remove(property);
        await _realEstateDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Property>> GetFavorites(int ownerID, PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties().Where(p => p.FavouritedByUsers.Any(u => u.ID == ownerID));

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();

    }

    public async Task<List<Property>> GetPropertiesByLocation(string city, string state, PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.ApprovalStatusID == (int)ApprovalStatusEnum.Approved &&
                   p.Location.City.ToLower() == city.ToLower() &&
                   p.Location.State.ToLower() == state.ToLower());

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();
    }

    public async Task<List<Property>> GetPropertiesByPincode(int zipCode, PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.ApprovalStatusID == (int)ApprovalStatusEnum.Approved &&
                   p.Location.ZipCode == zipCode);

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();
    }

    public async Task<List<Property>> GetPropertiesByPriceRange(double minPrice, double maxPrice, PropertyListingTypeEnum propertyListingType)
    {
        IQueryable<Property> propertyList = LoadAllProperties()
            .Where(p => p.ApprovalStatusID == (int)ApprovalStatusEnum.Approved &&
                   p.Price >= minPrice &&
                   p.Price <= maxPrice);

        if (propertyListingType != PropertyListingTypeEnum.All)
        {
            propertyList = propertyList.Where(p => p.PropertyStatusID == (int)propertyListingType);
        }

        return await propertyList.ToListAsync();
    }


}