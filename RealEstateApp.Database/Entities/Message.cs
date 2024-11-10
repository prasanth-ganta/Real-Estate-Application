
namespace RealEstateApp.Database.Entities;

public class Message
{
    public int ID { get; set; }
    public string Chat { get; set; }

    //ForignKeys
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }

    //Navigation Properties
    public User Sender { get; set; }
    public User Receiver { get; set; }
}
