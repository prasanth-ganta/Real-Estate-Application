using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class LoginDTO
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
}
