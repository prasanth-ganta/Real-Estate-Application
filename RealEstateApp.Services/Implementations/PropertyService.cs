using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PropertyService> logger
    )
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private int GetCurrentUserID()
    {
        try
        {
            var userIDClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userID");
            if (userIDClaim == null || string.IsNullOrEmpty(userIDClaim.Value))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return int.Parse(userIDClaim.Value);
        }
        catch (FormatException ex)
        {
            _logger.LogError($"Invalid user ID format in token: {ex.Message}");
            throw new UnauthorizedAccessException("Invalid user authentication token");
        }
    }

    public async Task<Response> CreateProperty(PropertyDTO property)
    {
        try
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var propertyLocation = new Location
            {
                ZipCode = property.Location.ZipCode,
                City = property.Location.City,
                State = property.Location.State,
                Country = property.Location.Country,
                GeoLocation = property.Location.GeoLocation,
            };

            var newProperty = new Property
            {
                Name = property.Name,
                Description = property.Description,
                Price = property.Price,
                PropertyTypeID = (int)property.PropertyType,
                SubPropertyTypeID = (int)property.PropertySubType,
                ApprovalStatusID = (int)ApprovalStatusEnum.Pending,
                PropertyStatusID = (int)property.PropertyStatus,
                Location = propertyLocation,
                OwnerID = GetCurrentUserID(),
            };

            await _propertyRepository.AddProperty(newProperty);
            return new Response(200, "Property Created Successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Authorization error while creating property: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while creating property: {ex.Message}");
            throw new InvalidOperationException("Failed to create property", ex);
        }
    }

    public async Task<Response> GetAllPendingProperties()
    {
        var result = await _propertyRepository.GetAllPendingProperties();
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200, responseProperty);
    }

    public async Task<Response> GetAllProperties(PropertyListingTypeEnum propertyListingType)
    {
        var result = await _propertyRepository.GetAllProperties(propertyListingType);
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200, responseProperty);
    }

    public async Task<Response> GetOwnedProperties(PropertyListingTypeEnum propertyListingType)
    {
        var result = await _propertyRepository.GetOwnedProperties(int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("userID").Value), propertyListingType);
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200, responseProperty);
    }

    public async Task<Response> SoftDeleteProperty(int ID)
    {
        string username = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
        bool isDeleted = await _propertyRepository.SoftDeleteProperty(ID, username);
        if (isDeleted == true) return new Response(200, "Deleted successfully");
        return new Response(404, "property was not found or already deleted");
    }

    private PropertyResponseDTO MappingProperty(Property property)
    {

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

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByLocation(string city, string state)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state))
            {
                throw new ArgumentException("City and state must be provided");
            }

            var properties = await _propertyRepository.GetPropertiesForBuyByLocation(city, state);
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties by location: {ex.Message}");
            throw new InvalidOperationException("Failed to search properties by location", ex);
        }
    }

    public async Task<Response> UpdatePropertyStatus(int ID, PropertyListingTypeEnum propertyListingType)
    {
        try
        {
            if (ID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var currentUserID = GetCurrentUserID();
            var userProperties = await _propertyRepository.GetOwnedProperties(currentUserID);
            var existingProperty = userProperties.FirstOrDefault(p => p.ID == ID);

            if (existingProperty == null)
            {
                throw new KeyNotFoundException($"Property with ID {ID} not found");
            }

            existingProperty.PropertyStatusID = (int)propertyListingType;
            await _propertyRepository.UpdatePropertyStatus(existingProperty);
            return new Response(200, "Property Updated Successfully");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Property not found: {ex.Message}");
            throw;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized property status update attempt: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating property status for ID {ID}: {ex.Message}");
            throw new InvalidOperationException("Failed to update property status", ex);
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByPincode(
        int zipCode
    )
    {
        try
        {
            if (zipCode <= 0)
            {
                throw new ArgumentException("Invalid ZIP code");
            }

            var properties = await _propertyRepository.GetPropertiesForBuyByPincode(zipCode);
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties by ZIP code {zipCode}: {ex.Message}");
            throw new InvalidOperationException($"Failed to search properties by ZIP code", ex);
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForBuyByPriceRange(
        double minPrice,
        double maxPrice
    )
    {
        try
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Price values cannot be negative");
            }

            if (minPrice >= maxPrice)
            {
                throw new ArgumentException("Minimum price must be less than maximum price");
            }

            var properties = await _propertyRepository.GetPropertiesForBuyByPriceRange(
                minPrice,
                maxPrice
            );
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid price range parameters: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error searching properties by price range ({minPrice}-{maxPrice}): {ex.Message}"
            );
            throw new InvalidOperationException("Failed to search properties by price range", ex);
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByLocation(
        string city,
        string state
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state))
            {
                throw new ArgumentException("City and state must be provided");
            }

            var properties = await _propertyRepository.GetPropertiesForRentByLocation(city, state);
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid location parameters: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error searching rental properties by location (city: {city}, state: {state}): {ex.Message}"
            );
            throw new InvalidOperationException(
                "Failed to search rental properties by location",
                ex
            );
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByPincode(
        int zipCode
    )
    {
        try
        {
            if (zipCode <= 0)
            {
                throw new ArgumentException("Invalid ZIP code");
            }

            var properties = await _propertyRepository.GetPropertiesForRentByPincode(zipCode);
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid ZIP code parameter: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error searching rental properties by ZIP code {zipCode}: {ex.Message}"
            );
            throw new InvalidOperationException(
                "Failed to search rental properties by ZIP code",
                ex
            );
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByPriceRange(
        double minPrice,
        double maxPrice
    )
    {
        try
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                throw new ArgumentException("Price values cannot be negative");
            }

            if (minPrice >= maxPrice)
            {
                throw new ArgumentException("Minimum price must be less than maximum price");
            }

            var properties = await _propertyRepository.GetPropertiesForRentByPriceRange(
                minPrice,
                maxPrice
            );
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid price range parameters: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error searching rental properties by price range ({minPrice}-{maxPrice}): {ex.Message}"
            );
            throw new InvalidOperationException(
                "Failed to search rental properties by price range",
                ex
            );
        }
    }

    public async Task<IEnumerable<PropertyResponseDTO>> SearchPropertiesForRentByName(
        string propertyName
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Property name must be provided");
            }

            var properties = await _propertyRepository.GetPropertiesForRentByName(propertyName);
            return _mapper.Map<IEnumerable<PropertyResponseDTO>>(properties);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid property name parameter: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error searching rental properties by name '{propertyName}': {ex.Message}"
            );
            throw new InvalidOperationException("Failed to search rental properties by name", ex);
        }
    }

    public async Task<Response> AddToFavorites(int propertyID)
    {
        try
        {
            var currentUserID = GetCurrentUserID();

            if (propertyID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var result = await _propertyRepository.AddToFavorites(currentUserID, propertyID);
            if (!result)
            {
                throw new InvalidOperationException(
                    "Property is already in favorites or does not exist"
                );
            }

            return new Response(201, "Property added to favorites successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized favorites addition attempt: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Failed to add property to favorites: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding property {propertyID} to favorites: {ex.Message}");
            throw new InvalidOperationException("Failed to add property to favorites", ex);
        }
    }

    public async Task<Response> RemoveFromFavorites(int propertyID)
    {
        try
        {
            var currentUserID = GetCurrentUserID();

            if (propertyID <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var result = await _propertyRepository.RemoveFromFavorites(currentUserID, propertyID);
            if (!result)
            {
                throw new KeyNotFoundException("Property not found in favorites");
            }

            return new Response(200, "Property removed from favorites successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized favorites removal attempt: {ex.Message}");
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Property not found in favorites: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing property {propertyID} from favorites: {ex.Message}");
            throw new InvalidOperationException("Failed to remove property from favorites", ex);
        }
    }

    public async Task<Response> GetFavorites(PropertyListingTypeEnum propertyListingType)
    {
        int userID=GetCurrentUserID();
        var result = await _propertyRepository.GetFavorites(userID, propertyListingType);
        List<PropertyResponseDTO> responseProperty = new List<PropertyResponseDTO>();
        foreach (var property in result)
        {
            responseProperty.Add(MappingProperty(property));
        }
        return new Response(200, responseProperty);
    }
}
