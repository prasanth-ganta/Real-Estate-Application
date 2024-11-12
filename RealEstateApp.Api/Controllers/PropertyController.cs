using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPost("CreateProperty")]
         public async Task<IActionResult> CreateProperty([FromForm] PropertyDTO property)
        {
            var result = await _propertyService.CreateProperty(property);
            return StatusCode(result.StatusCode,result.Value);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPost("GetOwnedProperties")]
        public async Task<IActionResult> GetOwnedProperties()
        {
            var result = await _propertyService.GetOwnedProperties();
            return StatusCode(result.StatusCode,result.Value);
        }
    }
}
