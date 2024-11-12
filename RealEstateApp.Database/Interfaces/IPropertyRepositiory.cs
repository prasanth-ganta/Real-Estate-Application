using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IPropertyRepository
{
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

    // Property retrieval methods
    Task<IEnumerable<Property>> GetAllProperties(int userId);
    Task<IEnumerable<Property>> GetOwnedPropertiesByUser (int userId);
    Task<IEnumerable<Property>> GetFavoritePropertiesByUser (int userId);

    //Method to change property status to "Sell"
    Task<bool> ChangePropertyStatusToSell(int propertyId);
}
