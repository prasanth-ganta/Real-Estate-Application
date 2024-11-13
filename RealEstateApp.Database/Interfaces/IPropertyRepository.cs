using RealEstateApp.Database.Entities;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
    public Task AddProperty(Property newProperty, string username);
    public Task<List<Property>> GetAllProperties(PropertyListingTypeEnum propertyStatus);
    public Task<List<Property>> GetOwnedProperties(int ownerID, PropertyListingTypeEnum propertyListingType);
    public Task<List<Property>> GetFavorites(int ownerID, PropertyListingTypeEnum propertyListingType);
    public Task<List<Property>> GetAllPendingProperties();
    public Task<bool> SoftDeleteProperty(int ID, string value);
    Task AddProperty(Property newProperty);
    Task<List<Property>> GetOwnedProperties(int ownerID);
    Task UpdatePropertyStatus(Property updatedProperty);
     // Buy Methods
    Task<IEnumerable<Property>> GetPropertiesForBuyByLocation(string city, string state);
    Task<IEnumerable<Property>> GetPropertiesForBuyByPincode(int zipCode);
    Task<IEnumerable<Property>> GetPropertiesForBuyByPriceRange(double minPrice, double maxPrice);
    
    // Rent Methods
    Task<IEnumerable<Property>> GetPropertiesForRentByLocation(string city, string state);
    Task<IEnumerable<Property>> GetPropertiesForRentByPincode(int zipCode);
    Task<IEnumerable<Property>> GetPropertiesForRentByPriceRange(double minPrice, double maxPrice);
    Task<IEnumerable<Property>> GetPropertiesForRentByName(string propertyName);

    //documents
    Task<bool> DeleteDocument(int documentID,int propertyID);
    
    //Favourites 
    Task<bool> AddToFavorites(int userID, int propertyID);
    Task<bool> RemoveFromFavorites(int userID, int propertyID);
}
