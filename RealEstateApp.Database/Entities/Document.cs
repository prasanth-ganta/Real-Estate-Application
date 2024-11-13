using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class Document
{
    //PrimaryKey
    public int ID { get; set; }
    
    [Url]
    public string Url {get;set;}
    public string Description {get;set;}
    public bool IsActive { get; set; } = true;

    //ForignKey
    public int PropertyId {get;set;}

    //Navigation Property
    public Property Property { get; set; }

}
