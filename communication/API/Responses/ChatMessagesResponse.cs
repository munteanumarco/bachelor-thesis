using BusinessLayer.DTOs;

namespace API.Responses;

public class ChatMessagesResponse : BaseResponse
{
    public IEnumerable<EnhancedMessageDto> ChatMessages { get; set; }

    private ChatMessagesResponse(bool isSuccess, IEnumerable<string> errorMessages, IEnumerable<EnhancedMessageDto> chatMessages)
        : base(isSuccess, errorMessages)
    {
        ChatMessages = chatMessages;
    }

    public static ChatMessagesResponse Success(IEnumerable<EnhancedMessageDto> chatMessages)
    {
        return new ChatMessagesResponse(true, new List<string>(), chatMessages);
    }

    public new static ChatMessagesResponse Failure(IEnumerable<string> errorMessages)
    {
        return new ChatMessagesResponse(false, errorMessages, null);
    }    
}