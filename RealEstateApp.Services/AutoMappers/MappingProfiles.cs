using AutoMapper;
using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs.RequestDTOs;
using RealEstateApp.Services.DTOs;
using RealEstateApp.Services.DTOs.ResponseDTOs;
namespace RealEstateApp.Services.AutoMappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDTO, User>();
        CreateMap<LoginDTO, User>();
        CreateMap<Message,MessageResponseDTO>()
            .ForMember(dest => dest.senderName, opt => opt.MapFrom(src => src.Sender.UserName)) 
            .ForMember(dest => dest.receiverName, opt => opt.MapFrom(src => src.Receiver.UserName))
            .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Chat));
        CreateMap<PropertySearchResultDto,Property>().ReverseMap();
        CreateMap<PropertyDTO,Property>().ReverseMap();
    }
}