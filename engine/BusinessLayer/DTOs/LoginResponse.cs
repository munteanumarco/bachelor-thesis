namespace BusinessLayer.DTOs;

public class LoginResponse
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }

    public LoginResponse(bool isSuccess, string errorMessage, string token)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Token = token;
    }

    public static LoginResponse Success(string token)
    {
        return new LoginResponse(true, null, token);
    }

    public static LoginResponse Failure(string errorMessage)
    {
        return new LoginResponse(false, errorMessage, "");
    }
}

