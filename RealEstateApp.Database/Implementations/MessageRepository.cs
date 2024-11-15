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

    public async Task<bool> SendMessage(Message message,string username)
    {
        User receiver = await _context.Users.FindAsync(message.ReceiverID);
        if(receiver == null || receiver.IsActive == false) return false;
        _context.Messages.Add(message);
        await _context.SaveChangesWithUserName(username);
        return true;
    }

    public async Task<List<Message>> GetMessagesBetweenUsers(int user1ID, int user2ID)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => ((m.SenderID == user1ID && m.ReceiverID == user2ID) ||
                        (m.SenderID == user2ID && m.ReceiverID == user1ID)) &&
                        (m.MessageVisibility == 0 || m.MessageVisibility == user1ID))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<bool> MarkMessageAsRead(int messageID,int receiverID)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageID && m.ReceiverID == receiverID && 
                            (m.MessageVisibility == 0 || m.MessageVisibility == receiverID));

        if (message != null)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<int> GetUnreadMessagesCount(int userID)
    {
        if(await _context.Users.FindAsync(userID) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages
            .Where(m => m.ReceiverID == userID && !m.IsRead &&
                        (m.MessageVisibility == 0 || m.MessageVisibility != userID))
            .CountAsync();
    }

    public async Task<List<Message>> GetAllUnreadMessages(int userID)
    {
        if(await _context.Users.FindAsync(userID) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages.Include(m=>m.Sender)
            .Where(m => m.ReceiverID == userID && !m.IsRead &&
                        (m.MessageVisibility == 0 || m.MessageVisibility != userID))
            .ToListAsync();
    }

    public async Task<bool> DeleteMessageForEverone(int messageID, int userID)
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

    public async Task<bool> DeleteAllMessagesBetweenUsers(int user1ID, int user2ID)
    {
        var messages = await _context.Messages
            .Where(m => (m.SenderID == user1ID && m.ReceiverID == user2ID) ||
                        (m.SenderID == user2ID && m.ReceiverID == user1ID))
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
                message.MessageVisibility = user2ID;
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
    public async Task<bool> DeleteMessageForMe(int messageID, int userID)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageID && (m.SenderID == userID || m.ReceiverID == userID));

        if (message != null)
        {
            int otherUserID = message.ReceiverID;
            if(message.ReceiverID == userID && message.SenderID == userID)
            {
                otherUserID = -1;
            }
            if(message.ReceiverID == userID)
            {
                otherUserID = message.SenderID;
            }
            message.MessageVisibility = otherUserID;
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }
}