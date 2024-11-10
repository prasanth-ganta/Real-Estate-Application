namespace RealEstateApp.Database.Entities;

public class FavouriteProperty
{
    public int ID { get; set; }
    public int PropertyId { get; set; }
    public int UserId { get; set; }
    public Property Property { get; set; }
    public User User { get; set; }
}
