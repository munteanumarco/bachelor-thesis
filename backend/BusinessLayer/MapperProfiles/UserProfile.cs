using AutoMapper;
using BusinessLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLayer.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserDto>();
    }
}