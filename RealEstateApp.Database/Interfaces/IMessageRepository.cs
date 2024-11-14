using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IMessageRepository
{
    public Task<bool> SendMessageAsync(Message message,string userName);
    public Task<bool> MarkMessageAsReadAsync(int messageId, int receiverId);
    public Task<int> GetUnreadMessagesCountAsync(int userId);
    public Task<List<Message>> GetAllUnreadMessagesAsync(int userId);
    public Task<List<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
    public Task<bool> DeleteAllMessagesBetweenUsersAsync(int user1Id, int user2Id);
    public Task<bool> DeleteMessageForEveroneAsync(int messageId, int userId);
    public Task<bool> DeleteMessageForMe(int messageId, int userId);
}
