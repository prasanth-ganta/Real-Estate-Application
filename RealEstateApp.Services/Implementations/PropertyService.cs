using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.Interfaces;
namespace RealEstateApp.Services.Implementations;
public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    // Buy Methods Implementation
    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByLocation(string city, string state)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByLocation(city, state);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPincode(int zipCode)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByPincode(zipCode);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPriceRange(double minPrice, double maxPrice)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByPriceRange(minPrice, maxPrice);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }


    // Rent Methods Implementation
    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByLocation(string city, string state)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByLocation(city, state);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPincode(int zipCode)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByPincode(zipCode);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPriceRange(double minPrice, double maxPrice)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByPriceRange(minPrice, maxPrice);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByName(string propertyName)
    {
        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByName(propertyName);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    // Property retrieval methods
    public async Task<IEnumerable<PropertySearchResultDto>> GetAllProperties (int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser (userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> GetFavoritePropertiesByUser (int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser (userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }
    
    public async Task<IEnumerable<PropertySearchResultDto>> GetOwnedPropertiesByUser (int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser (userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    // Method to change property status to "Sell"
    public async Task<bool> ChangePropertyStatusToSell(int propertyId)
    {
        return await _propertyRepository.ChangePropertyStatusToSell(propertyId);
    }
}