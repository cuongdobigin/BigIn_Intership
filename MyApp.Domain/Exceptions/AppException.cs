namespace Myapp.Domain.Exceptions;

public class AppException : Exception
{
    public int Code { get; }
    public int HttpStatusCode { get; }

    public AppException(ErrorCode errorCode)
        : base(ErrorDetails.Get(errorCode).Message)
    {
        var detail = ErrorDetails.Get(errorCode);
        Code = detail.Code;
        HttpStatusCode = detail.HttpStatusCode;
    }
}