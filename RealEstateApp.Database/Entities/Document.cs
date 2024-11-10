using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Database.Entities;

public class Document
{
    public int ID { get; set; }
    public int PropertyId {get;set;}
    [Url]
    public string Url {get;set;}
    public string Description {get;set;}
    public Property Property { get; set; }
}
