using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
using RealEstateApp.Services.ResponseType;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.Interfaces;

public interface IPropertyService
{
    public Task<Response> CreateProperty(PropertyDTO property);
    public Task<Response> GetOwnedProperties();
    Task<Response> UpdatePropertyStatus(int id, PropertyListingTypeEnum propertyListingType);

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

    //Documents methods
    Task<Response> AddDocument(DocumentDTO documentDTO,int PropertyId);
    Task<Response>DeleteDocument (int documentId,int propertyId);
    //Favourites
    Task<Response> AddToFavorites(int propertyId);
    Task<Response> RemoveFromFavorites(int propertyId);

}