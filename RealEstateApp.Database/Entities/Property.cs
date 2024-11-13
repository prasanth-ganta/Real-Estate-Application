namespace RealEstateApp.Database.Entities;

public class Property
{
    public int ID { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool IsActive { get; set; } = true;

     
    //ForignKeys
    public int PropertyTypeID { get; set; }  
    public int SubPropertyTypeID { get; set; }        
    public int OwnerID { get; set; }
    public int LocationID { get; set; }
    public int ApprovalStatusID { get; set; }
    public int PropertyStatusID { get; set; } 

    //Navigation Properties
    public PropertyType PropertyType { get; set; }
    public PropertySubType SubPropertyType { get; set; }
    public User Owner { get; set; }
    public Location Location { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public PropertyStatus PropertyStatus { get; set; } 
    public ICollection<Document> Documents { get; set; }
    public ICollection<User> FavouritedByUsers { get; set; }

}
