using AutoMapper;
using Microsoft.Extensions.Configuration;
using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
namespace RealEstateApp.Services.AutoMappers;

public class MappingProfiles : Profile
{
    private readonly IConfiguration _configuration;
    public MappingProfiles()
    {
        CreateMap<RegisterDTO, User>();
        CreateMap<LoginDTO, User>();
        CreateMap<Message,MessageResponseDTO>()
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName)) 
            .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.UserName))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Chat));
        CreateMap<User,UserDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        CreateMap<Document,DocumentResponseDTO>()
            .ForMember(dest => dest.File, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}