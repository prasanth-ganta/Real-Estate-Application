using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
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
        return new Response(200,"Property Created Successfully");
    }

    public async Task<Response> GetOwnedProperties()
    {
        var result = await _propertyRepository.GetOwnedProperties(int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value));
        return new Response(200,result);
    }
}
