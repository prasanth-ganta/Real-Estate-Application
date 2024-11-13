using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class Role
{
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<User> Users { get; set; }
}
