using AutoMapper;
using Microsoft.Extensions.Logging;
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
    private readonly ILoginUserDetailsService _loginUserDetailsService;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IMapper mapper,
        ILoginUserDetailsService loginUserDetailsService,
        ILogger<PropertyService> logger
    )
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _loginUserDetailsService = loginUserDetailsService;
        _logger = logger;
    }

    public async Task<Response> CreateProperty(PropertyDTO property)
    {
        try
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Location propertyLocation = new Location
            {
                ZipCode = property.Location.ZipCode,
                City = property.Location.City,
                State = property.Location.State,
                Country = property.Location.Country,
                GeoLocation = property.Location.GeoLocation,
            };

            Property newProperty = new Property
            {
                Name = property.Name,
                Description = property.Description,
                Price = property.Price,
                PropertyTypeID = (int)property.PropertyType,
                SubPropertyTypeID = (int)property.PropertySubType,
                ApprovalStatusID = (int)ApprovalStatusEnum.Pending,
                PropertyStatusID = (int)property.PropertyStatus,
                Location = propertyLocation,
                OwnerID = _loginUserDetailsService.GetCurrentUserID()
            };

            await _propertyRepository.AddProperty(newProperty,_loginUserDetailsService.GetCurrentUserName());
            return new Response(200, "Property Created Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating property: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> GetAllPendingProperties()
    {
        List<Property> result = await _propertyRepository.GetAllPendingProperties();
        List<PropertyResponseDTO> responseProperty = result.Select(property => MappingProperty(property)).ToList();
        return new Response(200, responseProperty);
    }

    public async Task<Response> ApproveProperty(int propertyID)
    {
        bool result = await _propertyRepository.ApproveProperty(propertyID);
        if(result == false)
        {
            return new Response(400, "Property Not Exit or already Approved");
        }
        return new Response(200, "Approved  Successfully");
    }

    public async Task<Response> GetAllProperties(PropertyListingTypeEnum propertyListingType)
    {
        List<Property> result = await _propertyRepository.GetAllProperties(propertyListingType);
        List<PropertyResponseDTO> responseProperty = result.Select(property => MappingProperty(property)).ToList();
        return new Response(200, responseProperty);
    }

    public async Task<Response> GetOwnedProperties(PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            List<Property> result = await _propertyRepository.GetOwnedProperties(userID, propertyListingType);
            List<PropertyResponseDTO> responseProperty = result.Select(property => MappingProperty(property)).ToList();
            return new Response(200, responseProperty);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting owned properties: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> SoftDeleteProperty(int PropertyID)
    {
        try
        {
            string username = _loginUserDetailsService.GetCurrentUserName();
            bool isDeleted = await _propertyRepository.SoftDeleteProperty(PropertyID, username);
            return isDeleted
                ? new Response(200, "Deleted successfully")
                : new Response(404, "Property was not found or already deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting property: {ex.Message}");
            throw;
        }
    }

    private PropertyResponseDTO MappingProperty(Property property)
    {
        LocationDTO propertyLocation = new LocationDTO
        {
            ZipCode = property.Location.ZipCode,
            City = property.Location.City,
            State = property.Location.State,
            Country = property.Location.Country,
            GeoLocation = property.Location.GeoLocation
        };

        return new PropertyResponseDTO
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
    }

    public async Task<Response> UpdatePropertyStatus(int propertyID, PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            if (propertyID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            int currentUserID = _loginUserDetailsService.GetCurrentUserID();
            List<Property> userProperties = await _propertyRepository.GetOwnedProperties(currentUserID);
            Property existingProperty = userProperties.FirstOrDefault(p => p.ID == propertyID);

            if (existingProperty == null)
            {
                throw new KeyNotFoundException($"Property with ID {propertyID} not found");
            }

            existingProperty.PropertyStatusID = (int)propertyListingType;
            await _propertyRepository.UpdatePropertyStatus(existingProperty);
            return new Response(200, "Property Updated Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating property status for ID {propertyID}: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> AddToFavorites(int propertyID)
    {
        try
        {
            int currentUserID = _loginUserDetailsService.GetCurrentUserID();

            if (propertyID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            bool result = await _propertyRepository.AddToFavorites(currentUserID, propertyID);
            if (!result)
            {
                throw new InvalidOperationException("Property is already in favorites or does not exist");
            }

            return new Response(201, "Property added to favorites successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding property {propertyID} to favorites: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> RemoveFromFavorites(int propertyID)
    {
        try
        {
            int currentUserID = _loginUserDetailsService.GetCurrentUserID();

            if (propertyID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            bool result = await _propertyRepository.RemoveFromFavorites(currentUserID, propertyID);
            if (!result)
            {
                throw new KeyNotFoundException("Property not found in favorites");
            }

            return new Response(200, "Property removed from favorites successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing property {propertyID} from favorites: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> GetFavorites(PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            int userID = _loginUserDetailsService.GetCurrentUserID();
            List<Property> result = await _propertyRepository.GetFavorites(userID, propertyListingType);
            List<PropertyResponseDTO> responseProperty = result.Select(property => MappingProperty(property)).ToList();
            return new Response(200, responseProperty);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while getting favorites: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> SearchPropertiesByLocation(string city, string state, PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City cannot be empty or null");
            }
            if (string.IsNullOrWhiteSpace(state))
            {
                throw new ArgumentException("State cannot be empty or null");
            }

            var properties = await _propertyRepository.GetPropertiesByLocation(city, state, propertyListingType);
            if (!properties.Any())
            {
                var listingTypeStr = propertyListingType == PropertyListingTypeEnum.All ? "sale and rental" :
                                    propertyListingType == PropertyListingTypeEnum.Rental ? "rental" : "sale";
                _logger.LogInformation($"No {listingTypeStr} properties found in {city}, {state}");
                return new Response(204, Enumerable.Empty<PropertyResponseDTO>());
            }

            var responseProperties = properties.Select(property => MappingProperty(property)).ToList();
            return new Response(200, responseProperties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties in {city}, {state}: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> SearchPropertiesByPincode(int zipCode, PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            if (zipCode <= 0 || zipCode > 999999)
            {
                throw new ArgumentException("Invalid ZIP code. Must be between 1 and 999999");
            }

            var properties = await _propertyRepository.GetPropertiesByPincode(zipCode, propertyListingType);
            if (!properties.Any())
            {
                var listingTypeStr = propertyListingType == PropertyListingTypeEnum.All ? "sale and rental" :
                                    propertyListingType == PropertyListingTypeEnum.Rental ? "rental" : "sale";
                _logger.LogInformation($"No {listingTypeStr} properties found with ZIP code {zipCode}");
                return new Response(204, Enumerable.Empty<PropertyResponseDTO>());
            }

            var responseProperties = properties.Select(property => MappingProperty(property)).ToList();
            return new Response(200, responseProperties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties by ZIP code {zipCode}: {ex.Message}");
            throw;
        }
    }

    public async Task<Response> SearchPropertiesByPriceRange(double minPrice, double maxPrice, PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            if (minPrice < 0)
            {
                throw new ArgumentException("Minimum price cannot be negative");
            }
            if (maxPrice <= 0)
            {
                throw new ArgumentException("Maximum price must be greater than zero");
            }
            if (minPrice >= maxPrice)
            {
                throw new ArgumentException("Minimum price must be less than maximum price");
            }

            var properties = await _propertyRepository.GetPropertiesByPriceRange(minPrice, maxPrice, propertyListingType);
            if (!properties.Any())
            {
                var listingTypeStr = propertyListingType == PropertyListingTypeEnum.All ? "sale and rental" :
                                    propertyListingType == PropertyListingTypeEnum.Rental ? "rental" : "sale";
                _logger.LogInformation($"No {listingTypeStr} properties found in price range {minPrice:C} - {maxPrice:C}");
                return new Response(204, Enumerable.Empty<PropertyResponseDTO>());
            }

            var responseProperties = properties.Select(property => MappingProperty(property)).ToList();
            return new Response(200, responseProperties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties in price range {minPrice:C} - {maxPrice:C}: {ex.Message}");
            throw;
        }
    }

}