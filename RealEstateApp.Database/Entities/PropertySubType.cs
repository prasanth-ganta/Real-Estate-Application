namespace RealEstateApp.Database.Entities;

public class PropertySubType
{
    public int Id { get; set; }  
    public string Name { get; set; }   
    
    // Navigation property 
    public ICollection<Property> Properties { get; set; }
}
