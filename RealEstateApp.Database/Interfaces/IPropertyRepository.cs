using RealEstateApp.Database.Entities;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
    public Task AddProperty(Property newProperty, string username);
    public Task<List<Property>> GetAllProperties(RetrivalOptionsEnum propertyStatus);
    public Task<List<Property>> GetOwnedProperties(int ownerId, RetrivalOptionsEnum retivalOption);
    public Task<List<Property>> GetAllPendingProperties();
    public Task<bool> SoftDeleteProperty(int id, string value);
    Task AddProperty(Property newProperty);
    Task<List<Property>> GetOwnedProperties(int ownerId);
    Task UpdatePropertyStatus(Property updatedProperty);
     // Buy Methods
    Task<IEnumerable<Property>> GetPropertiesForBuyByLocation(string city, string state);
    Task<IEnumerable<Property>> GetPropertiesForBuyByPincode(int zipCode);
    Task<IEnumerable<Property>> GetPropertiesForBuyByPriceRange(double minPrice, double maxPrice);
    //Task<IEnumerable<Property>> GetPropertiesForBuyByType(int propertyTypeId);
    
    // Rent Methods
    Task<IEnumerable<Property>> GetPropertiesForRentByLocation(string city, string state);
    Task<IEnumerable<Property>> GetPropertiesForRentByPincode(int zipCode);
    Task<IEnumerable<Property>> GetPropertiesForRentByPriceRange(double minPrice, double maxPrice);
    Task<IEnumerable<Property>> GetPropertiesForRentByName(string propertyName);

    //documents
    Task<bool> AddDocument(Document document,int propertyId);
    Task<bool> DeleteDocument(int documentId,int propertyId);
    
    //Favourites 
    Task<bool> AddToFavorites(int userId, int propertyId);
    Task<bool> RemoveFromFavorites(int userId, int propertyId);
}
