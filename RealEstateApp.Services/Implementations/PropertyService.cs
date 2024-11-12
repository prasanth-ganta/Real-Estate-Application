using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.Implementations;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response> CreateProperty(PropertyDTO property)
    {
        var propertyLocation = new Location
        {
            ZipCode = property.Location.ZipCode,
            City = property.Location.City,
            State = property.Location.State,
            Country = property.Location.Country,
            GeoLocation = property.Location.GeoLocation
        };

        var newProperty = new Property
        {
            Name = property.Name,
            Description = property.Description,
            Price = property.Price,
            PropertyTypeId = (int)property.PropertyType,
            SubPropertyTypeId = (int)property.PropertySubType,
            ApprovalStatusId = (int)ApprovalStatusEnum.Pending,
            PropertyStatusId = (int)property.PropertyStatus,
            Location = propertyLocation,

            OwnerId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value)
        };
        await _propertyRepository.AddProperty(newProperty);
        return new Response(200, "Property Created Successfully");
    }

    public async Task<Response> GetOwnedProperties()
    {
        var result = await _propertyRepository.GetOwnedProperties(int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value));
        return new Response(200, result);
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
    public async Task<IEnumerable<PropertySearchResultDto>> GetAllProperties(int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser(userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> GetFavoritePropertiesByUser(int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser(userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    public async Task<IEnumerable<PropertySearchResultDto>> GetOwnedPropertiesByUser(int userId)
    {

        IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetFavoritePropertiesByUser(userId);
        return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
    }

    // Method to change property status to "Sell"
    public async Task<bool> ChangePropertyStatusToSell(int propertyId)
    {
        return await _propertyRepository.ChangePropertyStatusToSell(propertyId);
    }
}