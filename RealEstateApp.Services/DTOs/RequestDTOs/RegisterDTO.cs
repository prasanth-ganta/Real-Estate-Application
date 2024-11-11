using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class RegisterDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

}
