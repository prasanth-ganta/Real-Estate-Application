using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class DocumentDTO
{
    [Required]
    public IFormFile uploadDocument {get;set;}

    [Required]
    public string Description {get;set;}
}
