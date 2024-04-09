namespace API.Responses;

public class LoginResponse : BaseResponse
{
    public string Token { get; set; }

    private LoginResponse(bool isSuccess, IEnumerable<string> errorMessages, string token)
        : base(isSuccess, errorMessages)
    {
        Token = token;
    }

    public static LoginResponse Success(string token)
    {
        return new LoginResponse(true, new List<string>(), token);
    }

    public new static LoginResponse Failure(IEnumerable<string> errorMessages)
    {
        return new LoginResponse(false, errorMessages, "");
    }
}

