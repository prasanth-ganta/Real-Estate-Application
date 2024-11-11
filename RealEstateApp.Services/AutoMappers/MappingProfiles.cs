using AutoMapper;
using RealEstateApp.Database.Entities;
using RealEstateApp.Services.DTOs.RequestDTOs;

namespace RealEstateApp.Services.AutoMappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDTO,User>();
        CreateMap<LoginDTO,User>();
    }
}