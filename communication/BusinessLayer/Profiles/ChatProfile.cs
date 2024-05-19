using AutoMapper;
using BusinessLayer.DTOs;
using DataAccessLayer.Entities;

namespace BusinessLayer.Profiles;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<ChatEvent, ChatDetailsDto>()
            .ForMember(dest => dest.ParticipantsCount, opt => opt.Ignore());

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.ChatId, opt => opt.MapFrom(src => src.ChatMessage.ChatId));

        CreateMap<Message, EnhancedMessageDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.MessageText))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}