namespace RealEstateApp.Database.Entities;

public class Document
{
    //PrimaryKey
    public int ID { get; set; }
    
    public string FileName {get;set;}
    public string Description {get;set;}

    //ForignKey
    public int PropertyID {get;set;}

    //Navigation Property
    public Property Property { get; set; }

}
