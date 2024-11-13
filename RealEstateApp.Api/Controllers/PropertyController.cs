using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Utility.Enumerations;

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



        


    }
}
