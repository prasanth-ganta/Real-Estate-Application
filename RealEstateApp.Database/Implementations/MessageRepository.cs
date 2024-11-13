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
        User receiver = await _context.Users.FindAsync(message.ReceiverId) ;
        if(receiver == null || receiver.IsActive == false) return false;
        _context.Messages.Add(message);
        await _context.SaveChangesWithUserName(username);
        return true;
    }

    public async Task<List<Message>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id))
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<bool> MarkMessageAsReadAsync(int messageId,int receiverId)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageId && m.ReceiverId == receiverId);

        if (message != null)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<int> GetUnreadMessagesCountAsync(int userId)
    {
        if(await _context.Users.FindAsync(userId) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages
            .Where(m => m.ReceiverId == userId && !m.IsRead)
            .CountAsync();
        
    }

    public async Task<List<Message>> GetAllUnreadMessagesAsync(int userId)
    {
        if(await _context.Users.FindAsync(userId) == null)
        {
            throw new NullReferenceException("No such user exist");
        }
        return await _context.Messages.Include(m=>m.Sender)
            .Where(m => m.ReceiverId == userId && !m.IsRead)
            .ToListAsync();
    }


    public async Task<bool> DeleteMessageAsync(int messageId, int userId)
    {
        var message = await _context.Messages
            .FirstOrDefaultAsync(m => m.ID == messageId && m.SenderId == userId);

        if (message != null)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }
    public async Task<bool> DeleteAllMessagesBetweenUsersAsync(int user1Id, int user2Id)
    {
        var messages = _context.Messages
            .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                        (m.SenderId == user2Id && m.ReceiverId == user1Id))
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