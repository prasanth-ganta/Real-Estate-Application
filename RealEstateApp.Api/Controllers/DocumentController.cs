using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public DocumentController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpPost("addDocuments")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDocument([FromForm] DocumentDTO document, int propertyId)
    {
        var result = await _propertyService.AddDocument(document, propertyId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("deleteDocuments")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDocument(int documentId, int propertyId)
    {
        var result = await _propertyService.DeleteDocument(documentId, propertyId);
        return StatusCode(result.StatusCode, result);
    }

}
