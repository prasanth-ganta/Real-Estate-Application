namespace RealEstateApp.Services.DTOs.ResponseDTOs;
public class PropertySearchResultDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string PropertyType { get; set; }
    public string SubPropertyType { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public int ZipCode { get; set; }
    public string GeoLocation { get; set; }
}