using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLayer.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDto, EmergencyAppUser>();
        CreateMap<EmergencyAppUser, UserDto>().ReverseMap();
    }
}