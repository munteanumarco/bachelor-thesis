namespace API.Responses;


public class ConfirmEmailResponse
{
    public bool IsSuccess { get; set; }
    public IEnumerable<string> ErrorMessages { get; set; }

    public ConfirmEmailResponse(bool isSuccess, IEnumerable<string> errorMessages)
    {
        IsSuccess = isSuccess;
        ErrorMessages = errorMessages;
    }

    public static ConfirmEmailResponse Success()
    {
        return new ConfirmEmailResponse(true, new List<string>());
    }

    public static ConfirmEmailResponse Failure(IEnumerable<string> errorMessages)
    {
        return new ConfirmEmailResponse(false, errorMessages);
    }
}
