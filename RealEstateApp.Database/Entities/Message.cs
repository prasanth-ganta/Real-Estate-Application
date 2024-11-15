
namespace RealEstateApp.Database.Entities;

public class Message
{
    public int ID { get; set; }
    public string Chat { get; set; }
    public bool IsRead { get; set; }
    public int MessageVisibility { get; set; }
    public DateTime Timestamp { get; set; }

    //ForignKeys
    public int SenderID { get; set; }
    public int ReceiverID { get; set; }

    //Navigation Properties
    public User Sender { get; set; }
    public User Receiver { get; set; }
}
