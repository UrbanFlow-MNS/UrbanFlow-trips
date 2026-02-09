namespace UrbanFlow_trips.Exceptions;

public class NotFoundException : Exception
{
    public int ErrorCode { get; set; }
    public NotFoundException(string paramName, int errorCode) : base (paramName)    
    {
        ErrorCode = errorCode;
    }
}