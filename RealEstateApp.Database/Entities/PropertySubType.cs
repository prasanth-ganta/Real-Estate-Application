using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class PropertySubType
{
    public int Id { get; set; }  

    [Required]
    public string Name { get; set; }   
    public bool IsActive { get; set; } = true;

    
    // Navigation property 
    public ICollection<Property> Properties { get; set; }
}
