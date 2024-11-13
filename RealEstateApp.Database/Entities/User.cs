using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class User
{
    //Primary Key
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";  

    [EmailAddress]
    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }
    public bool IsActive { get; set; } = true;


    //Namvigation Properties
    public ICollection<Role> Roles { get; set; }
    public ICollection<Property> OwnedProperties { get; set; }
    public ICollection<Message> ReceivedMessages { get; set; }
    public ICollection<Message> SentMessages { get; set; }
    public ICollection<Property> FavouriteProperties { get; set; }
    
}
