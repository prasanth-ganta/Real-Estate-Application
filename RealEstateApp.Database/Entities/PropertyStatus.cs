namespace RealEstateApp.Database.Entities;

public class PropertyStatus
{
    public int ID { get; set; }
    public string Status { get; set; }

    public ICollection<Property> Properties { get; set; }
}
