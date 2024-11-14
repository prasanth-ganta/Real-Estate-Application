using RealEstateApp.Database.Entities;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
    public Task<bool> AddProperty(Property newProperty, string username);
    public Task<List<Property>> GetAllProperties(PropertyListingTypeEnum propertyStatus);
    public Task<List<Property>> GetOwnedProperties(int ownerID, PropertyListingTypeEnum propertyListingType);
    public Task<List<Property>> GetFavorites(int ownerID, PropertyListingTypeEnum propertyListingType);
    public Task<List<Property>> GetAllPendingProperties();
    public Task<bool> ApproveProperty(int propertyID);
    public Task<bool> SoftDeleteProperty(int propertyID, string value);
    Task<List<Property>> GetOwnedProperties(int ownerID);
    Task UpdatePropertyStatus(Property updatedProperty);
    
    //Favourites 
    Task<bool> AddToFavorites(int userID, int propertyID);
    Task<bool> RemoveFromFavorites(int userID, int propertyID);
    
    //search
    Task<List<Property>> GetPropertiesByLocation(string city, string state, PropertyListingTypeEnum propertyListingType);
    Task<List<Property>> GetPropertiesByPincode(int zipCode, PropertyListingTypeEnum propertyListingType);
    Task<List<Property>> GetPropertiesByPriceRange(double minPrice, double maxPrice, PropertyListingTypeEnum propertyListingType);
}
    
