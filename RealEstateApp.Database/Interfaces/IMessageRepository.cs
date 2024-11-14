using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IMessageRepository
{
    public Task<bool> SendMessage(Message message,string userName);
    public Task<bool> MarkMessageAsRead(int messageID, int receiverID);
    public Task<int> GetUnreadMessagesCount(int userID);
    public Task<List<Message>> GetAllUnreadMessages(int userID);
    public Task<List<Message>> GetMessagesBetweenUsers(int user1ID, int user2ID);
    public Task<bool> DeleteAllMessagesBetweenUsers(int user1ID, int user2ID);
    public Task<bool> DeleteMessageForEverone(int messageID, int userID);
    public Task<bool> DeleteMessageForMe(int messageID, int userID);
}
