using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<Property> Properties { get; set; }
    public ICollection<Message> ReceivedMessages { get; set; }
    public ICollection<Message> SentMessages { get; set; }

    public ICollection<FavouriteProperty> FavouriteProperties { get; set; }
}
