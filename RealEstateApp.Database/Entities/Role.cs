using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class Role
{
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
}
