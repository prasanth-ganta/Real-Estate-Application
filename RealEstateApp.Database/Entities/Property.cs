namespace RealEstateApp.Database.Entities;

public class Property
{
    public int ID { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
     
    //ForignKeys
    public int PropertyTypeId { get; set; }  
    public int SubPropertyTypeId { get; set; } 
    public int OwnerId { get; set; }
    public int LocationId { get; set; }
    public int ApprovalStatusId { get; set; }
    public int StatusId { get; set; }
    public int PropertyStatusId { get; set; }  


    //Navigation Properties
    public PropertyType PropertyType { get; set; }
    public PropertySubType SubPropertyType { get; set; }
    public User Owner { get; set; }
    public Location Location { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public PropertyStatus PropertyStatus { get; set; } 
    public ICollection<Document> Documents { get; set; }

}
