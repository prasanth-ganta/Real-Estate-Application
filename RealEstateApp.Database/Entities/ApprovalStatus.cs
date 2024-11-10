namespace RealEstateApp.Database.Entities;

public class ApprovalStatus
{
    public int ID { get; set; }
    public string Status { get; set; }

    //Navigation Property
    public ICollection<Property> properties { get; set; }

}
