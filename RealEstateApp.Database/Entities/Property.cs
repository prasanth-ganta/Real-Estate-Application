using RealEstateApp.Utility.Enumerations;

namespace RealEstateApp.Database.Entities;

public class Property
{
    public int ID { get; set; }
    public int OwnerId { get; set; }
    public int LocationId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public PropertyType PropertyType { get; set; }
    public SubPropertyType SubPropertyType { get; set; }
    public ApprovalStatus ApprovedStatus { get; set; }
    public Status Status { get; set; }
    public Document Document { get; set; }
    public Location Location { get; set; }
    public User Owner { get; set; } 
    public ICollection<Message> Messages { get; set; }
    public ICollection<Document> Documents { get; set; }

}
