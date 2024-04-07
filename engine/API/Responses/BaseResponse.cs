namespace API.Responses;


public class BaseResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }

    public BaseResponse(bool isSuccess, IEnumerable<string> errorMessages)
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
    }

    public static BaseResponse Success()
    {
        return new BaseResponse(true, new List<string>());
    }

    public static BaseResponse Failure(IEnumerable<string> errorMessages)
    {
        return new BaseResponse(false, errorMessages);
    }
}
