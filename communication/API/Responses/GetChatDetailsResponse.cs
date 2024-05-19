using BusinessLayer.DTOs;

namespace API.Responses;

public class GetChatDetailsResponse : BaseResponse
{
    public ChatDetailsDto Data { get; set; }
    
    private GetChatDetailsResponse(bool isSuccess, IEnumerable<string> errorMessages, ChatDetailsDto chatDetailsDto)
        : base(isSuccess, errorMessages)
    {
        Data = chatDetailsDto;
    }

    public static GetChatDetailsResponse Success(ChatDetailsDto chatDetailsDto)
    {
        return new GetChatDetailsResponse(true, new List<string>(), chatDetailsDto);
    }

    public new static GetChatDetailsResponse Failure(IEnumerable<string> errorMessages)
    {
        return new GetChatDetailsResponse(false, errorMessages, null);
    }    
}