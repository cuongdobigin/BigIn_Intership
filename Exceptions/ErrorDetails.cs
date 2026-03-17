namespace webApi.Domain.Exceptions;

public record ErrorDetail
{
    public int Code { get; init; }
    public string Message { get; init; } = string.Empty;
    public int HttpStatusCode { get; init; }
}

public static class ErrorDetails
{
    private static readonly Dictionary<ErrorCode, ErrorDetail> _errorDetails = new()
    {
        { ErrorCode.ProductNotFound,       new ErrorDetail { Code = 1001, Message = "The specified product was not found.", HttpStatusCode = 404 } },
        { ErrorCode.PRODUCT_PRICE_INVALID, new ErrorDetail { Code = 1002, Message = "The product price is invalid.", HttpStatusCode = 400 } },
        { ErrorCode.PRODUCT_NAME_REQUIRED, new ErrorDetail { Code = 1003, Message = "The product name is required.", HttpStatusCode = 400 } },
        { ErrorCode.ACCOUNT_NOT_FOUND ,new ErrorDetail{ Code = 1004, Message = "Account not found.", HttpStatusCode = 400 } },
        { ErrorCode.PASSWORD_ERROR ,new ErrorDetail{ Code = 1005, Message = "Incorrect password.", HttpStatusCode = 400 } },
        { ErrorCode.SECRETKEY_NOT_FOUND ,new ErrorDetail{ Code = 1006,Message = "SecretKey not found.", HttpStatusCode = 400 } },
        { ErrorCode.EXPIRESECONDS_NOT_FOUND,new ErrorDetail{ Code = 1007,Message = "ExpireSeconds not found.", HttpStatusCode = 400 } }
    };

    public static ErrorDetail Get(ErrorCode code)
        => _errorDetails.TryGetValue(code, out var detail)
            ? detail
            : new ErrorDetail { Code = 9999, Message = "Unknown error", HttpStatusCode = 500 };
}