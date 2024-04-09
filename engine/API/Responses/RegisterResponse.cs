
using BusinessLayer.DTOs.UserManagement;

namespace API.Responses;

public class RegisterResponse : BaseResponse
{
    public UserDto User { get; set; }

    private RegisterResponse(bool isSuccess, IEnumerable<string> errorMessages, UserDto userDto)
        : base(isSuccess, errorMessages)
    {
        User = userDto;
    }

    public static RegisterResponse Success(UserDto userDto)
    {
        return new RegisterResponse(true, new List<string>(), userDto);
    }

    public new static RegisterResponse Failure(IEnumerable<string> errorMessages)
    {
        return new RegisterResponse(false, errorMessages, null);
    }
}