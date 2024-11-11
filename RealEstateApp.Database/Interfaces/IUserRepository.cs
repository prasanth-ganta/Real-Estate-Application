using RealEstateApp.Database.Entities;

namespace RealEstateApp.Database.Interfaces;

public interface IUserRepository
{
    public Task<bool> AddUser(User user);
    public Task<bool> ContainsUser(string email);
    public Task<User> GetUser(string email);
}
