using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;

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
    public async Task<IActionResult> CreateProperty([FromForm] PropertyDTO property)
    {
        var result = await _propertyService.CreateProperty(property);
        return StatusCode(result.StatusCode, result.Value);
    }

    [Authorize(Roles = "User,Admin")]
    [HttpPost("GetOwnedProperties")]
    public async Task<IActionResult> GetOwnedProperties()
    {
        var result = await _propertyService.GetOwnedProperties();
        return StatusCode(result.StatusCode, result.Value);
    }



    // Buy Endpoints
    [HttpGet("buy/by-location")]
    public async Task<IActionResult> SearchPropertiesForBuyByLocation(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByLocation(city, state);
        return Ok(properties);
    }

    [HttpGet("buy/by-pincode")]
    public async Task<IActionResult> SearchPropertiesForBuyByPincode([FromQuery] int zipCode)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByPincode(zipCode);
        return Ok(properties);
    }

    [HttpGet("buy/by-price-range")]
    public async Task<IActionResult> SearchPropertiesForBuyByPriceRange(
        [FromQuery] double minPrice,
        [FromQuery] double maxPrice)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForBuyByPriceRange(minPrice, maxPrice);
        return Ok(properties);
    }

    // Rent Endpoints
    [HttpGet("rent/by-location")]
    public async Task<IActionResult> SearchPropertiesForRentByLocation(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForRentByLocation(city, state);
        return Ok(properties);
    }

    [HttpGet("rent/by-pincode")]
    public async Task<IActionResult> SearchPropertiesForRentByPincode([FromQuery] int zipCode)
    {
        var properties = await _propertyService.SearchPropertiesForRentByPincode(zipCode);
        return Ok(properties);
    }

    [HttpGet("rent/by-price-range")]
    public async Task<IActionResult> SearchPropertiesForRentByPriceRange(
        [FromQuery] double minPrice,
        [FromQuery] double maxPrice)
    {
        var properties = await _propertyService.SearchPropertiesForRentByPriceRange(minPrice, maxPrice);
        return Ok(properties);
    }

    [HttpGet("rent/by-name")]
    public async Task<IActionResult> SearchPropertiesForRentByName([FromQuery] string propertyName)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.SearchPropertiesForRentByName(propertyName);
        return Ok(properties);
    }

    [HttpGet("exclude/{userId}")]
    public async Task<IActionResult> GetAllProperties(int userId)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.GetAllProperties(userId);
        return Ok(properties);
    }

    [HttpGet("owned/{userId}")]
    public async Task<IActionResult> GetOwnedPropertiesByUser(int userId)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.GetOwnedPropertiesByUser(userId);
        return Ok(properties);
    }

    [HttpGet("favorites/{userId}")]
    public async Task<IActionResult> GetFavoritePropertiesByUser(int userId)
    {
        IEnumerable<PropertySearchResultDto> properties = await _propertyService.GetFavoritePropertiesByUser(userId);
        return Ok(properties);
    }

    [HttpPut("sell/{propertyId}")]
    public async Task<IActionResult> ChangePropertyStatusToSell(int propertyId)
    {
        bool result = await _propertyService.ChangePropertyStatusToSell(propertyId);
        if (result)
        {
            return Ok(new { Message = "Property status changed to Sell." });
        }
        return NotFound(new { Message = "Property not found or already for sale." });
    }

}
