using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class PropertyType
{
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;

    
    // Navigation property
    public ICollection<Property> Properties { get; set; } 
}
