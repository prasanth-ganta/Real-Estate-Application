using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Interfaces;

public interface IUserService
{
    public Response GetAllUsers();
    public Task<Response> Login(LoginDTO login);
    public Task<Response> Register(RegisterDTO registeredData);


}
