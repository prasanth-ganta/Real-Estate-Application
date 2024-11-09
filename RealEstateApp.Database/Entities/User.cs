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
}
