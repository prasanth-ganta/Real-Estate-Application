using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Services.DTOs;

public class LocationDTO
{
    [Required]
    public int ZipCode { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string State { get; set; }

    [Required]
    public string Country { get; set; }
    public string GeoLocation { get; set; }
}
