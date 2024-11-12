using RealEstateApp.Services.DTOs;

namespace RealEstateApp.Services.Interfaces;
public interface IPropertyService
{
    // Buy Methods
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByLocation(string city, string state);
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPincode(int zipCode);
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPriceRange(double minPrice, double maxPrice);
    //Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByType(int propertyTypeId);
    
    // Rent Methods
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByLocation(string city, string state);
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPincode(int zipCode);
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPriceRange(double minPrice, double maxPrice);
    Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByName(string propertyName);

    // Property retrieval methods
    Task<IEnumerable<PropertySearchResultDto>> GetAllProperties (int userId);
    Task<IEnumerable<PropertySearchResultDto>> GetFavoritePropertiesByUser (int userId);
    Task<IEnumerable<PropertySearchResultDto>> GetOwnedPropertiesByUser (int userId);

    //Method to change property status to "Sell"
    Task<bool> ChangePropertyStatusToSell(int propertyId);
}