using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;
using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("CreateProperty")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProperty([FromForm] PropertyDTO property)
    {
        Response result = await _propertyService.CreateProperty(property);
        return StatusCode(result.StatusCode, result.Value);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "User,Admin")]
    [HttpGet("GetOwnedProperties")]
    public async Task<IActionResult> GetOwnedProperties(PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.GetOwnedProperties(propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpGet("GetAllProperties")]
    public async Task<IActionResult> GetAllProperties(PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.GetAllProperties(propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Admin/GetAllPendingProperties")]
    public async Task<IActionResult> GetAllPendingProperties()
    {
        Response result = await _propertyService.GetAllPendingProperties();
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpPut("approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveProperty(int propertyID)
    {
        Response result = await _propertyService.ApproveProperty(propertyID);
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpDelete("{ID}")]
    public async Task<IActionResult> DeleteProperty(int propertyID)
    {
        Response result = await _propertyService.SoftDeleteProperty(propertyID);
        return StatusCode(result.StatusCode, result.Value);

    }

    [HttpPut("UpdateProperty{ID}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePropertyStatus(int propertyID, PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.UpdatePropertyStatus(propertyID, propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpPost("addFavourites/{propertyID}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddToFavorites(int propertyID)
    {
        Response result = await _propertyService.AddToFavorites(propertyID);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("removeFavourites/{propertyID}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFromFavorites(int propertyID)
    {
        Response result = await _propertyService.RemoveFromFavorites(propertyID);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("favorites")]
    public async Task<IActionResult> GetFavorites(PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.GetFavorites(propertyListingType);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("search/location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesByLocation([FromQuery] string city, [FromQuery] string state, [FromQuery] PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.SearchPropertiesByLocation(city, state, propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpGet("search/pincode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesByPincode([FromQuery] int zipCode, [FromQuery] PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.SearchPropertiesByPincode(zipCode, propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpGet("search/price-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesByPriceRange([FromQuery] double minPrice, [FromQuery] double maxPrice, [FromQuery] PropertyListingTypeEnum propertyListingType)
    {
        Response result = await _propertyService.SearchPropertiesByPriceRange(minPrice, maxPrice, propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }
}