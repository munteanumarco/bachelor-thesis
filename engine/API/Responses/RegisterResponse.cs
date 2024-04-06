using BusinessLayer.DTOs;

namespace API.Responses;

public class RegisterResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }
    public UserDto User { get; set; }

    public RegisterResponse(bool isSuccess, IEnumerable<string> errorMessages, UserDto userDto)
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        User = userDto;
    }

    public static RegisterResponse Success(UserDto userDto)
    {
        return new RegisterResponse(true, new List<string>(), userDto);
    }

    public static RegisterResponse Failure(IEnumerable<string> errorMessages)
    {
        return new RegisterResponse(false, errorMessages, null);
    }
}