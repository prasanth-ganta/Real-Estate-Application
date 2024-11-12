using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class PropertySubType
{
    public int ID { get; set; }  

    [Required]
    public string Name { get; set; }   
    
    // Navigation property 
    public ICollection<Property> Properties { get; set; }
}
