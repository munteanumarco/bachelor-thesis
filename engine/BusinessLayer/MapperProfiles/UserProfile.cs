using AutoMapper;
using BusinessLayer.DTOs.UserManagement;
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