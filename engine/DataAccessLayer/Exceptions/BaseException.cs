namespace DataAccessLayer.Exceptions;

public class BaseException : Exception
{
    public BaseException() { }
    public BaseException(string message) : base(message) { }
}