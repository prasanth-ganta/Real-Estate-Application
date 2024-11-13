using Microsoft.EntityFrameworkCore;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations;

public class UserRepository : IUserRepository
{
private readonly RealEstateDbContext _context;

    public UserRepository(RealEstateDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUser (User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await _context.Roles.FindAsync(roleId);
    }

    public async Task<User> GetUser (string userName)
    {
        try
        {
            return await _context.Users.Include(roles => roles.Roles).FirstAsync(u => u.UserName == userName);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.Include(u => u.OwnedProperties).ToList();
    }
}
