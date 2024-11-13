using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
using RealEstateApp.Services.Interfaces;
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
        var result = await _propertyService.CreateProperty(property);
        return StatusCode(result.StatusCode, result.Value);
    }

        [Authorize(Roles ="User,Admin")]
        [HttpGet("GetOwnedProperties")]
        public async Task<IActionResult> GetOwnedProperties(RetrivalOptionsEnum retivalOption)
        {
            var result = await _propertyService.GetOwnedProperties(retivalOption);
            return StatusCode(result.StatusCode,result.Value);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpGet("GetAllProperties")]
        public async Task<IActionResult> GetAllProperties(RetrivalOptionsEnum retivalOption)
        {
            var result = await _propertyService.GetAllProperties(retivalOption);
            return StatusCode(result.StatusCode,result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/GetAllPendingProperties")]
        public async Task<IActionResult> GetAllPendingProperties()
        {
            var result = await _propertyService.GetAllPendingProperties();
            return StatusCode(result.StatusCode,result.Value);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var result = await _propertyService.SoftDeleteProperty(id);
            return StatusCode(result.StatusCode,result.Value);

        }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("GetOwnedProperties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOwnedProperties()
    {
        var result = await _propertyService.GetOwnedProperties();
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpPut("UpdateProperty{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePropertyStatus(int id, PropertyListingTypeEnum propertyListingType)
    {
        var result = await _propertyService.UpdatePropertyStatus(id, propertyListingType);
        return StatusCode(result.StatusCode, result.Value);
    }

    [HttpGet("buy/by-location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForBuyByLocation(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByLocation(city, state);
        return Ok(properties);
    }

    [HttpGet("buy/by-pincode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForBuyByPincode([FromQuery] int zipCode)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByPincode(zipCode);
        return Ok(properties);
    }

    [HttpGet("buy/by-price-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForBuyByPriceRange(
        [FromQuery] double minPrice,
        [FromQuery] double maxPrice)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByPriceRange(minPrice, maxPrice);
        return Ok(properties);
    }

    [HttpGet("rent/by-location")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForRentByLocation(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForRentByLocation(city, state);
        return Ok(properties);
    }

    [HttpGet("rent/by-pincode")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForRentByPincode([FromQuery] int zipCode)
    {
        var properties = await _propertyService.SearchPropertiesForRentByPincode(zipCode);
        return Ok(properties);
    }

    [HttpGet("rent/by-price-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForRentByPriceRange(
        [FromQuery] double minPrice,
        [FromQuery] double maxPrice)
    {
        var properties = await _propertyService.SearchPropertiesForRentByPriceRange(minPrice, maxPrice);
        return Ok(properties);
    }
    
    [HttpGet("rent/by-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchPropertiesForRentByName([FromQuery] string propertyName)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForRentByName(propertyName);
        return Ok(properties);
    }
    
    [HttpPost("addFavourites/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddToFavorites(int propertyId)
    {
        var result = await _propertyService.AddToFavorites(propertyId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("removeFavourites/{propertyId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFromFavorites(int propertyId)
    {
        var result = await _propertyService.RemoveFromFavorites(propertyId);
        return StatusCode(result.StatusCode, result);
    }
}