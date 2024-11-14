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
        await _context.SaveChangesWithUserName(user.UserName);
        return true;
    }

    public async Task<Role> GetRole(int roleID)
    {
        return await _context.Roles.FindAsync(roleID);
    }

    public async Task<User> GetUser (string username)
    {
        return await _context.Users.Include(roles => roles.Roles).FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.Include(u => u.OwnedProperties).ToListAsync();
    }

    public async Task<bool> DeactivateUser(int userID, string username)
    {
        User user = await _context.Users.Include(u => u.Roles).FirstAsync(u => u.ID == userID);
        var roles = user.Roles;
        foreach (var role in roles)
        {
            if(role.Name == "Admin")
            {
                throw new InvalidOperationException("Can't deactivate other admin");
            }
        }
        if(user == null || !user.IsActive)
        {
            return false;
        }
        user.IsActive = false;
        _context.SaveChangesWithUserName(username);
        return true;
    }

    public async Task<bool> ActivateUser(int userID, string username)
    {
        User user = await _context.Users.FirstAsync(u => u.ID == userID);
    
        if(user == null)
        {
            throw new NullReferenceException("No such user Exist");
        }
        else if(user.IsActive){
            return false;
        }
        user.IsActive = true;
        _context.SaveChangesWithUserName(username);
        return true;
    }
}
