using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.DTOs.EmergencyEvent;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace BusinessLayer.MapperProfiles;

public class EmergencyEventProfile : Profile
{
    public EmergencyEventProfile()
    {
        CreateMap<EmergencyEventCreationDto, EmergencyEvent>();
        CreateMap<EmergencyEvent, EmergencyEventDto>().ReverseMap();
        CreateMap(typeof(PagedResult<>), typeof(PagedResultDto<>));
    }
}