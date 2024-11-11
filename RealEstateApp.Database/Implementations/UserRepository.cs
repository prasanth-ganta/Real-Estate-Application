using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;

namespace RealEstateApp.Database.Implementations;

public class UserRepository : IUserRepository
{
    public Task<bool> AddUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ContainsUser(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUser(string email)
    {
        throw new NotImplementedException();
    }
}
