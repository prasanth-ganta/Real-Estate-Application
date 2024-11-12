using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IUserRepository
{
    public Task<bool> AddUser(User user);
    public List<User> GetAllUsers();
    public Task<Role> GetRole(int roleId);
    public Task<User> GetUser(string userName);
}
