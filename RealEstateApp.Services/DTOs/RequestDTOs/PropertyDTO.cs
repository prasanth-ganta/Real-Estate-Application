using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Services.DTOs.RequestDTOs;

public class PropertyDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public PropertyTypeEnum PropertyType { get; set; }
    public PropertySubTypeEnum PropertySubType { get; set; }
    public LocationDTO Location { get; set; }
    public PropertyStatusEnum PropertyStatus { get; set; }
}
