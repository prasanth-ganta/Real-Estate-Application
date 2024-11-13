using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IUserRepository
{
    public Task<bool> AddUser(User user);
    public Task<List<User>> GetAllUsers();
    public Task<Role> GetRole(int roleID);
    public Task<User> GetUser(string username);
    public Task<bool> DeactivateUser(int userID, string username);
    public Task<bool> ActivateUser(int userID, string username);
}
