namespace BusinessLayer.Helpers;

public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public T Data { get; }
    public IEnumerable<string> Errors { get; }

    private OperationResult(bool isSuccess, T data, IEnumerable<string> errors)
    {
        IsSuccess = isSuccess;
        Data = data;
        Errors = errors ?? new List<string>();
    }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>(true, data, new List<string>());
    }

    public static OperationResult<T> Failure(IEnumerable<string> errors)
    {
        return new OperationResult<T>(false, default, errors);
    }
}
