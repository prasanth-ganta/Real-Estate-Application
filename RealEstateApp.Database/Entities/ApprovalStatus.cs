using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class ApprovalStatus
{
    public int ID { get; set; }
    [Required]
    public string Status { get; set; }

    //Navigation Property
    public ICollection<Property> Properties { get; set; }

}
