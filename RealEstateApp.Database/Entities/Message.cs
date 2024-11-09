
namespace RealEstateApp.Database.Entities;

public class Message
{
    public int ID { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public int PropertyId { get; set; }
    public string Chat { get; set; }
}
