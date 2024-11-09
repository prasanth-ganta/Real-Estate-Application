namespace RealEstateApp.Database.Entities;

public class Location
{
    public int ID { get; set; }
    public int ZipCode { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string GeoLocation { get; set; }
}
