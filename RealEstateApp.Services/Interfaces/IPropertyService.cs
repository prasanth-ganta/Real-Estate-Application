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
    public Task<Response> ApproveProperty(int propertyID);
    public Task<Response> SoftDeleteProperty(int ID);
    Task<Response> UpdatePropertyStatus(int ID, PropertyListingTypeEnum propertyListingType);
    Task<Response> AddToFavorites(int propertyID);
    Task<Response> RemoveFromFavorites(int propertyID);
    Task<Response>GetFavorites(PropertyListingTypeEnum propertyListingType);

    //search
    Task<Response> SearchPropertiesByPriceRange(double minPrice, double maxPrice, PropertyListingTypeEnum propertyListingType);
    Task<Response> SearchPropertiesByPincode(int zipCode, PropertyListingTypeEnum propertyListingType);
    Task<Response> SearchPropertiesByLocation(string city, string state, PropertyListingTypeEnum propertyListingType);
}