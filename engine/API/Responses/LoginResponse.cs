namespace API.Responses;

public class LoginResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }
    public string Token { get; set; }

    public LoginResponse(bool isSuccess, IEnumerable<string> errorMessages, string token)
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
        Token = token;
    }

    public static LoginResponse Success(string token)
    {
        return new LoginResponse(true, new List<string>(), token);
    }

    public static LoginResponse Failure(IEnumerable<string> errorMessages)
    {
        return new LoginResponse(false, errorMessages, "");
    }
}

