using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
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
        await _propertyRepository.AddProperty(newProperty, _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value);
    return new Response(200,"Property Created Successfully");
    }

    public async Task<Response> GetAllPendingProperties()
    {
        var result = await _propertyRepository.GetAllPendingProperties();
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200,responseProperty);
    }

    public async Task<Response> GetAllProperties(RetrivalOptionsEnum retivalOption)
    {
        var result = await _propertyRepository.GetAllProperties(retivalOption);
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200,responseProperty);    
    }
    
    public async Task<Response> GetOwnedProperties(RetrivalOptionsEnum retivalOption)
    {
        var result = await _propertyRepository.GetOwnedProperties(int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("userId").Value), retivalOption);
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200,responseProperty); 
    }

    public async Task<Response> SoftDeleteProperty(int id)
    {
        string username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
        bool isDeleted = await _propertyRepository.SoftDeleteProperty(id, username);
        if( isDeleted == true ) return new Response(200,"Deleted successfully"); 
        return new Response(404,"property was not found or already deleted");
    }

    private PropertyResponseDTO MappingProperty(Property property){

        var propertyLocation = new LocationDTO
        {
            ZipCode = property.Location.ZipCode,
            City = property.Location.City,
            State = property.Location.State,
            Country = property.Location.Country,
            GeoLocation = property.Location.GeoLocation
        };
    
        var responseProperty = new PropertyResponseDTO 
        { 
            Name = property.Name,
            Description = property.Description,
            Price = property.Price, 
            PropertyType = property.PropertyType.Name, 
            PropertySubType = property.SubPropertyType.Name, 
            PropertyStatus = property.PropertyStatus.Status, 
            ApprovalStatus = property.ApprovalStatus.Status, 
            Location = propertyLocation,
            OwnerName = property.Owner.FullName
        };
        return responseProperty;
    }
}
