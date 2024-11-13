using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IUserRepository
{
    public Task<bool> AddUser(User user);
    public Task<List<User>> GetAllUsers();
    public Task<Role> GetRole(int roleId);
    public Task<User> GetUser(string username);
    public Task<bool> DeactivateUser(int userId, string username);
    public Task<bool> ActivateUser(int userId, string username);
}
