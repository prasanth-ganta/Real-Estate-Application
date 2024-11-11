using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
