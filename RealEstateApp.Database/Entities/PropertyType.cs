namespace RealEstateApp.Database.Entities;

public class PropertyType
{
    public int ID { get; set; }
    public string Name { get; set; }
    
    // Navigation property
    public ICollection<Property> Properties { get; set; } 
}
