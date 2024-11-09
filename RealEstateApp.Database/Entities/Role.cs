namespace RealEstateApp.Database.Entities;

public class Role
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ICollection<User> users { get; set; }
}
