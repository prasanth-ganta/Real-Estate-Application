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
            .Where(m => ((m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id)) &&
                        (m.MessageVisibility == 0 || m.MessageVisibility == user1Id))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<bool> MarkMessageAsReadAsync(int messageID,int receiverID)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageId && m.ReceiverId == receiverId && 
                            (m.MessageVisibility == 0 || m.MessageVisibility == receiverId));

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
            .Where(m => m.ReceiverId == userId && !m.IsRead &&
                        (m.MessageVisibility == 0 || m.MessageVisibility != userId))
            .CountAsync();
        
    }

    public async Task<List<Message>> GetAllUnreadMessagesAsync(int userID)
    {
        if(await _context.Users.FindAsync(userID) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages.Include(m=>m.Sender)
            .Where(m => m.ReceiverId == userId && !m.IsRead &&
                        (m.MessageVisibility == 0 || m.MessageVisibility != userId))
            .ToListAsync();
    }

    public async Task<bool> DeleteMessageForEveroneAsync(int messageId, int userId)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageID && m.SenderID == userID);

        if (message != null)
        {
            message.MessageVisibility = -1;
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }

    public async Task<bool> DeleteAllMessagesBetweenUsersAsync(int user1Id, int user2Id)
    {
        var messages = await _context.Messages
            .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id))
            .ToListAsync();

        if (!messages.Any() )
        {
            throw new InvalidOperationException("Nothing to clear.");
        }

        bool visibilityChanged = false;

        foreach (var message in messages)
        {
            if (message.MessageVisibility > 0)
            {
                message.MessageVisibility = -1;
                visibilityChanged = true;
            }
            else if (message.MessageVisibility == -1)
            {
                throw new InvalidOperationException("Nothing to clear.");
            }
            else if (message.MessageVisibility == 0)
            {
                message.MessageVisibility = user2Id;
                visibilityChanged = true;
            }
        }

        if (visibilityChanged)
        {
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
    public async Task<bool> DeleteMessageForMe(int messageId, int userId)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageId && (m.SenderId == userId || m.ReceiverId == userId));

        if (message != null)
        {
            int otherUserId = message.SenderId;
            if(message.ReceiverId == userId && message.SenderId == userId)
            {
                otherUserId = -1;
            }
            if(message.ReceiverId == userId)
            {
                otherUserId = message.SenderId;
            }
            message.MessageVisibility = otherUserId;
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }
}