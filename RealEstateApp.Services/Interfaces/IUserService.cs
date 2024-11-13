using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces;

public interface IUserService
{
    public Task<Response> GetAllUsers();
    public Task<Response> Login(LoginDTO login);
    public Task<Response> Register(RegisterDTO registeredData);
    public Task<Response> DeactivateUser(int userID);
    public Task<Response> ActivateUser(int userID);
}
