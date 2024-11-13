using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class ApprovalStatus
{
    public int ID { get; set; }
    
    [Required]
    public string Status { get; set; }
    public bool IsActive { get; set; } = true;

    //Navigation Property
    public ICollection<Property> Properties { get; set; }


}
