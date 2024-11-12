using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;

namespace RealEstateApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public Response GetAllUsers()
    {
        return new Response(200,_userRepository.GetAllUsers());
    }

    public async Task<Response> Login(LoginDTO loginUser)
    {
        
        User user = await  _userRepository.GetUser(loginUser.UserName);
        if (user== null)
        {
            return new Response(409, "User does not Exist! Need To register First");
        }
        if (BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
        {
            string token = GenerateJwtToken(user);
            return new Response(200, new { Login_status = "Logged in", Token = token });
        }
        
        return new Response(401, "InValid Credentials");

    }

    public async Task<Response> Register(RegisterDTO user)
    {

        if (await _userRepository.GetUser(user.UserName) != null)
        {
            return new Response(409, "User Already Exist");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Password = hashedPassword;
        User newUser = _mapper.Map<User>(user);
        newUser.Roles = new List<Role>();
        Role role = await _userRepository.GetRole(1);
        newUser.Roles.Add(role);
        await _userRepository.AddUser(newUser);
        return new Response(201, new { user = user.Email, Message = " Successfully Registered" });
    }
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var Credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var payload = new List<Claim>
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim("userId",user.ID.ToString())
        };
        foreach (Role role in user.Roles)
        {
            payload.Add(new Claim(ClaimTypes.Role, role.Name));
        } 
        var token = new JwtSecurityToken
        (
            issuer: _configuration["JwtSettings:Issuer"],
            claims: payload,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpiryMinutes")),
            signingCredentials: Credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
