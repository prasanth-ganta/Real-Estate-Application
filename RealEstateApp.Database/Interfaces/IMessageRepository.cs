using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IMessageRepository
{
    public Task<bool> SendMessageAsync(Message message,string userName);
    public Task<bool> MarkMessageAsReadAsync(int messageID, int receiverID);
    public Task<int> GetUnreadMessagesCountAsync(int userID);
    public Task<List<Message>> GetAllUnreadMessagesAsync(int userID);
    public Task<List<Message>> GetMessagesBetweenUsersAsync(int user1ID, int user2ID);
    public Task<bool> DeleteMessageAsync(int messageID,int userID);
    public Task<bool> DeleteAllMessagesBetweenUsersAsync(int user1ID, int user2ID);
}
