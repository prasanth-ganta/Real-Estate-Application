using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Database.Entities;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.Interfaces;
using RealEstateApp.Services.ResponseType;  

namespace RealEstateApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILoginUserDetailsService _loginDetails;

    public UserService(IMapper mapper, IUserRepository userRepository, IConfiguration configuration, ILoginUserDetailsService loginUserDetails)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
        _loginDetails = loginUserDetails;
    }

    public async Task<Response> GetAllUsers()
    {
        List<User> users =await _userRepository.GetAllUsers();
        if(users == null)
        {
            return new Response(204, "No Users");
        }
        List<UserDTO> usersList = _mapper.Map<List<UserDTO>>(users);
        return new Response(200, usersList);
    }

    public async Task<Response> Login(LoginDTO loginUser)
    {
        User user = await _userRepository.GetUser(loginUser.UserName);
        if (user == null)
        {
            return new Response(409, "User does not Exist! Need To register First");
        }
        else if(!user.IsActive)
        {
            return new Response(409, "User is Deactivated by admin. Contact Admin.");
        }
        if (BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
        {
            string token = GenerateJwtToken(user);
            return new Response(200, new { Login_status = "Logged in", Token =$"Bearer {token}" });
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

    public async Task<Response> DeactivateUser(int userID)
    {
        if( await _userRepository.DeactivateUser(userID,_loginDetails.GetCurrentUserName()))
        {
            return new Response(200,"Successfully Deactivated");
        }
        return new Response(400,"User doesn't exit or Already Deactivated");
    }

    public async Task<Response> ActivateUser(int userID)
    {
        if( await _userRepository.ActivateUser(userID,_loginDetails.GetCurrentUserName()))
        {
            return new Response(200,"Successfully Activated");
        }
        return new Response(400,"User Already Active");
    }
    
    private string GenerateJwtToken(User user)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])
        );
        SigningCredentials Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> payload = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("userID", user.ID.ToString()),
        };
        foreach (Role role in user.Roles)
        {
            payload.Add(new Claim(ClaimTypes.Role, role.Name));
        }
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            claims: payload,
            expires: DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("JwtSettings:ExpiryMinutes")
            ),
            signingCredentials: Credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
