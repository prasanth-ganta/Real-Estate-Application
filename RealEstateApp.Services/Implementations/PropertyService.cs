// using AutoMapper;
// using Microsoft.AspNetCore.Http;
// using RealEstateApp.Database.Entities;
// using RealEstateApp.Database.Interfaces;
// using RealEstateApp.Services.DTOs.RequestDTOs;
// using RealEstateApp.Services.DTOs.ResponseDTOs;
// using RealEstateApp.Services.Interfaces;
// using RealEstateApp.Services.ResponseType;
// using RealEstateApp.Utility.Enumerations;

// namespace RealEstateApp.Services.Implementations;

// public class PropertyService : IPropertyService
// {
//     private readonly IPropertyRepository _propertyRepository;
//     private readonly IMapper _mapper;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public PropertyService(IPropertyRepository propertyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
//     {
//         _propertyRepository = propertyRepository;
//         _mapper = mapper;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     public async Task<Response> CreateProperty(PropertyDTO property)
//     {
//         var propertyLocation = new Location
//         {
//             ZipCode = property.Location.ZipCode,
//             City = property.Location.City,
//             State = property.Location.State,
//             Country = property.Location.Country,
//             GeoLocation = property.Location.GeoLocation
//         };

//         var newProperty = new Property
//         {
//             Name = property.Name,
//             Description = property.Description,
//             Price = property.Price,
//             PropertyTypeId = (int)property.PropertyType,
//             SubPropertyTypeId = (int)property.PropertySubType,
//             ApprovalStatusId = (int)ApprovalStatusEnum.Pending,
//             PropertyStatusId = (int)property.PropertyStatus,
//             Location = propertyLocation,

//             OwnerId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value)
//         };
//         await _propertyRepository.AddProperty(newProperty);
//         return new Response(200, "Property Created Successfully");
//     }

//     public async Task<Response> GetOwnedProperties()
//     {
//         var result = await _propertyRepository.GetOwnedProperties(int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value));
//         return new Response(200, result);
//     }

//     public async Task<Response> UpdatePropertyStatus(int id, PropertyListingTypeEnum propertyListingType)
//     {
//         var currentUserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value);
//         IEnumerable<Property> userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);
//         Property existingProperty = userProperties.FirstOrDefault(p => p.ID == id);
//         if (existingProperty==null)
//         {
//             return new Response(404, "Property not found");
//         }

//         existingProperty.StatusId=(int)propertyListingType;
//         await _propertyRepository.UpdatePropertyStatus(existingProperty);
//         return new Response(200, "Property Updated Successfully");
//     }




//     // Buy Methods Implementation
//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByLocation(string city, string state)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByLocation(city, state);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }

//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPincode(int zipCode)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByPincode(zipCode);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }

//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPriceRange(double minPrice, double maxPrice)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForBuyByPriceRange(minPrice, maxPrice);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }


//     // Rent Methods Implementation
//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByLocation(string city, string state)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByLocation(city, state);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }

//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPincode(int zipCode)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByPincode(zipCode);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }

//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPriceRange(double minPrice, double maxPrice)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByPriceRange(minPrice, maxPrice);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }

//     public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByName(string propertyName)
//     {
//         IEnumerable<Property> properties = (IEnumerable<Property>)await _propertyRepository.GetPropertiesForRentByName(propertyName);
//         return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
//     }


//     //Documents
//     public async Task<Response> AddDocument(DocumentDTO documentDTO,int propertyId)
//     {
//         var currentUserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value);
//         IEnumerable<Property> userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);

//         if (!userProperties.Any(p => p.ID == propertyId))
//         {
//             new Response(403, "You don't have permission to add documents to this property");
//         }

//         var document = new Document
//         {
//             Url = documentDTO.Url,
//             Description = documentDTO.Description,
//             PropertyId = propertyId,
//         };

//         var result = await _propertyRepository.AddDocument(document,propertyId);
//         return new Response(201,"Document added successfully");
//     }


//     public async Task<Response> DeleteDocument(int documentId, int propertyId)
//     {
//         var currentUserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value);
//         IEnumerable<Property> userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);

//         if (!userProperties.Any(p => p.ID == propertyId))
//         {
//            return new Response(403, "You don't have permission to delete documents to this property");
//         }

//         var deleted = await _propertyRepository.DeleteDocument(documentId, propertyId);
//         if (!deleted)
//         {
//             return new Response(404, "Document not found");
//         }

//         return new Response(200, "Document deleted successfully");
//     }

//     //Favourites
//     public async Task<Response> AddToFavorites(int propertyId)
//     {
//         var currentUserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value);
//         IEnumerable<Property> userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);

//         if (userProperties.Any(p => p.ID == propertyId))
//         {
//            return new Response(400, "Property is already in favorites or failed to add");
//         }

//         var result = await _propertyRepository.AddToFavorites(currentUserId, propertyId);
//         if (!result)
//         {
//             return new Response(400, "Property is already in favorites or failed to add");
//         }

//         return new Response(201, "Property added to favorites successfully");
//     }

//     public async Task<Response> RemoveFromFavorites(int propertyId)
//     {
//         var currentUserId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("userId").Value);

//         var result = await _propertyRepository.RemoveFromFavorites(currentUserId, propertyId);
//         if (!result)
//         {
//             return new Response(404, "Property not found in favorites");
//         }

//         return new Response(200, "Property removed from favorites successfully");
//     }

// }

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
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

    private int GetCurrentUserId()
    {
        try
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId");
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return int.Parse(userIdClaim.Value);
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
                PropertyTypeId = (int)property.PropertyType,
                SubPropertyTypeId = (int)property.PropertySubType,
                ApprovalStatusId = (int)ApprovalStatusEnum.Pending,
                PropertyStatusId = (int)property.PropertyStatus,
                Location = propertyLocation,
                OwnerId = GetCurrentUserId(),
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

    public async Task<Response> GetOwnedProperties()
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _propertyRepository.GetOwnedProperties(userId);
            return new Response(200, result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Authorization error while fetching owned properties: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while fetching owned properties: {ex.Message}");
            throw new InvalidOperationException("Failed to retrieve owned properties", ex);
        }
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByLocation(
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

            var properties = await _propertyRepository.GetPropertiesForBuyByLocation(city, state);
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties by location: {ex.Message}");
            throw new InvalidOperationException("Failed to search properties by location", ex);
        }
    }

    public async Task<Response> UpdatePropertyStatus(
        int id,
        PropertyListingTypeEnum propertyListingType
    )
    {
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var currentUserId = GetCurrentUserId();
            var userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);
            var existingProperty = userProperties.FirstOrDefault(p => p.ID == id);

            if (existingProperty == null)
            {
                throw new KeyNotFoundException($"Property with ID {id} not found");
            }

            existingProperty.StatusId = (int)propertyListingType;
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
            _logger.LogError($"Error updating property status for ID {id}: {ex.Message}");
            throw new InvalidOperationException("Failed to update property status", ex);
        }
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPincode(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching properties by ZIP code {zipCode}: {ex.Message}");
            throw new InvalidOperationException($"Failed to search properties by ZIP code", ex);
        }
    }

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForBuyByPriceRange(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
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

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByLocation(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
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

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPincode(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
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

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByPriceRange(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
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

    public async Task<IEnumerable<PropertySearchResultDto>> SearchPropertiesForRentByName(
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
            return _mapper.Map<IEnumerable<PropertySearchResultDto>>(properties);
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

    public async Task<Response> AddDocument(DocumentDTO documentDTO, int propertyId)
    {
        try
        {
            if (documentDTO == null)
            {
                throw new ArgumentNullException(nameof(documentDTO));
            }

            var currentUserId = GetCurrentUserId();
            var userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);

            if (!userProperties.Any(p => p.ID == propertyId))
            {
                throw new UnauthorizedAccessException(
                    "You don't have permission to add documents to this property"
                );
            }

            var document = new Document
            {
                Url = documentDTO.Url,
                Description = documentDTO.Description,
                PropertyId = propertyId,
            };

            await _propertyRepository.AddDocument(document, propertyId);
            return new Response(201, "Document added successfully");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized document addition attempt: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding document to property {propertyId}: {ex.Message}");
            throw new InvalidOperationException("Failed to add document to property", ex);
        }
    }

    public async Task<Response> DeleteDocument(int documentId, int propertyId)
    {
        try
        {
            if (documentId <= 0)
            {
                throw new ArgumentException("Invalid document ID");
            }

            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var currentUserId = GetCurrentUserId();
            var userProperties = await _propertyRepository.GetOwnedProperties(currentUserId);

            if (!userProperties.Any(p => p.ID == propertyId))
            {
                throw new UnauthorizedAccessException(
                    "You don't have permission to delete documents from this property"
                );
            }

            var deleted = await _propertyRepository.DeleteDocument(documentId, propertyId);
            if (!deleted)
            {
                throw new KeyNotFoundException(
                    $"Document with ID {documentId} not found for property {propertyId}"
                );
            }

            _logger.LogInformation(
                $"Successfully deleted document {documentId} from property {propertyId}"
            );
            return new Response(200, "Document deleted successfully");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning($"Invalid argument while deleting document: {ex.Message}");
            throw;
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning($"Unauthorized document deletion attempt: {ex.Message}");
            throw;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning($"Document not found: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Error deleting document {documentId} from property {propertyId}: {ex.Message}"
            );
            throw new InvalidOperationException($"Failed to delete document", ex);
        }
    }

    public async Task<Response> AddToFavorites(int propertyId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var result = await _propertyRepository.AddToFavorites(currentUserId, propertyId);
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
            _logger.LogError($"Error adding property {propertyId} to favorites: {ex.Message}");
            throw new InvalidOperationException("Failed to add property to favorites", ex);
        }
    }

    public async Task<Response> RemoveFromFavorites(int propertyId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            if (propertyId <= 0)
            {
                throw new ArgumentException("Invalid property ID");
            }

            var result = await _propertyRepository.RemoveFromFavorites(currentUserId, propertyId);
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
            _logger.LogError($"Error removing property {propertyId} from favorites: {ex.Message}");
            throw new InvalidOperationException("Failed to remove property from favorites", ex);
        }
    }
}
