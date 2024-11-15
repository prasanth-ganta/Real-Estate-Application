using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Api.Controllers; 

[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost("addDocuments")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDocument([FromForm] DocumentDTO document, int propertyID)
    {
        Response result = await _documentService.AddDocument(document, propertyID);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("deleteDocuments")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDocument(int documentID)
    {

        Response result = await _documentService.DeleteDocument(documentID);
        return StatusCode(result.StatusCode, result);
    }
}
