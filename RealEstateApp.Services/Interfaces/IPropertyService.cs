using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
using RealEstateApp.Services.ResponseType;
using RealEstateApp.Utility.Enumerations;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.Interfaces;

public interface IPropertyService
{
    public Task<Response> CreateProperty(PropertyDTO property);
    public Task<Response> GetAllProperties(PropertyListingTypeEnum propertyListingType);
    public Task<Response> GetOwnedProperties(PropertyListingTypeEnum propertyListingType);
    public Task<Response> GetAllPendingProperties();
    public Task<Response> SoftDeleteProperty(int ID);
    Task<Response> UpdatePropertyStatus(int ID, PropertyListingTypeEnum propertyListingType);

    // Buy Methods
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByLocation(string city, string state);
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByPincode(int zipCode);
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByPriceRange(double minPrice, double maxPrice);
    //Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByType(int propertyTypeID);

    // Rent Methods
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByLocation(string city, string state);
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByPincode(int zipCode);
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByPriceRange(double minPrice, double maxPrice);
    Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByName(string propertyName);

    //Favourites
    Task<Response> AddToFavorites(int propertyID);
    Task<Response> RemoveFromFavorites(int propertyID);
    //Task GetFavorites(PropertyListingTypeEnum propertyListingType);
    Task<Response>GetFavorites(PropertyListingTypeEnum propertyListingType);
}