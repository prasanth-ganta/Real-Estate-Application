using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class DocumentDTO
{
    [Required]
    [Url]
    public string Url {get;set;}

    [Required]
    public string Description {get;set;}
}
