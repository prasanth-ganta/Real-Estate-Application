using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations;

public class MessageRepository : IMessageRepository
{
    private readonly RealEstateDbContext _context;

    public MessageRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SendMessageAsync(Message message,string username)
    {
        User receiver = await _context.Users.FindAsync(message.ReceiverID);
        if(receiver == null || receiver.IsActive == false) return false;
        _context.Messages.Add(message);
        await _context.SaveChangesWithUserName(username);
        return true;
    }

    public async Task<List<Message>> GetMessagesBetweenUsersAsync(int user1ID, int user2ID)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderID == user1ID && m.ReceiverID == user2ID) ||
                        (m.SenderID == user2ID && m.ReceiverID == user1ID))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<bool> MarkMessageAsReadAsync(int messageID,int receiverID)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageID && m.ReceiverID == receiverID);

        if (message != null)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<int> GetUnreadMessagesCountAsync(int userID)
    {
        if(await _context.Users.FindAsync(userID) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages
            .Where(m => m.ReceiverID == userID && !m.IsRead)
            .CountAsync();
        
    }

    public async Task<List<Message>> GetAllUnreadMessagesAsync(int userID)
    {
        if(await _context.Users.FindAsync(userID) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages.Include(m=>m.Sender)
            .Where(m => m.ReceiverID == userID && !m.IsRead)
            .ToListAsync();
    }


    public async Task<bool> DeleteMessageAsync(int messageID, int userID)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageID && m.SenderID == userID);

        if (message != null)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }
    public async Task<bool> DeleteAllMessagesBetweenUsersAsync(int user1ID, int user2ID)
    {
        var messages = _context.Messages
            .Where(m => (m.SenderID == user1ID && m.ReceiverID == user2ID) ||
                        (m.SenderID == user2ID && m.ReceiverID == user1ID))
            .ToList();

        if (messages.Any())
        {
            _context.Messages.RemoveRange(messages);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}