namespace Myapp.Domain.Exceptions;

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
        { ErrorCode.EXPIRESECONDS_NOT_FOUND,new ErrorDetail{ Code = 1007,Message = "ExpireSeconds not found.", HttpStatusCode = 400 } },
        { ErrorCode.EMAIL_EXISTED ,new ErrorDetail{ Code = 1008,Message = "Email already exists.", HttpStatusCode = 400 } },
        { ErrorCode.INCORRECT_PASSWORD ,new ErrorDetail{ Code = 1009,Message = "Incorrect Password.", HttpStatusCode = 400 } },
        { ErrorCode.INCORRECT_EMAIL,new ErrorDetail{ Code = 1010,Message = "Incorrect Email.", HttpStatusCode = 400 } },
        { ErrorCode.OLD_PASSWORD_ERROR ,new ErrorDetail{ Code = 1011,Message = "The old password is incorrect.", HttpStatusCode = 400 } },
        {ErrorCode.REFRESH_TOKEN_INVALID ,new ErrorDetail{ Code = 1012,Message = "Refresh token is invalid.", HttpStatusCode = 401 } },
        { ErrorCode.USER_EXISTED ,new ErrorDetail{ Code = 1014,Message = "User exists.", HttpStatusCode = 400 } },
        {ErrorCode.UNAUTHORIZED,new ErrorDetail{ Code = 1015,Message = "Unauthorized.", HttpStatusCode = 401 } },
        {ErrorCode.TYPE_BOOK_EXISTED,new ErrorDetail{ Code = 1016,Message = "Type book already exists.", HttpStatusCode = 400 } },
        { ErrorCode.TYPE_BOOK_NOT_FOUND ,new ErrorDetail{ Code = 1017,Message = "Type book not found.", HttpStatusCode = 400 } },
        { ErrorCode.INCORRECT_PRICE ,new ErrorDetail{ Code = 1018,Message = "The price must be greater than 0..", HttpStatusCode = 400 } },
        { ErrorCode.INORRECT_QUANTITY  ,new ErrorDetail{ Code = 1019,Message = "The quantity must be greater than 0..", HttpStatusCode = 400 } },
        { ErrorCode.BOOK_EXISTED ,new ErrorDetail{ Code = 1020,Message = "Book already exists.", HttpStatusCode = 400 } },
        { ErrorCode.BOOK_NOT_FOUND  ,new ErrorDetail{ Code = 1021,Message = "Book not found.", HttpStatusCode = 400 } },
        { ErrorCode.INCORRECT_IMAGE ,new ErrorDetail{ Code = 1022,Message = "There must be at least one photo..", HttpStatusCode = 400 } },
        { ErrorCode.IMAGE_NOT_FOUND  ,new ErrorDetail{ Code = 1023,Message = "Image not found.", HttpStatusCode = 400 } },
        {ErrorCode.REVIEW_NOT_FOUND,new ErrorDetail{ Code = 1024,Message = "Review not found.", HttpStatusCode = 400 } },
        { ErrorCode.CART_NOT_FOUND ,new ErrorDetail{ Code = 1025,Message = "Cart not found.", HttpStatusCode = 400 } },
        { ErrorCode.CART_ITEM_ALREADY_EXISTS ,new ErrorDetail{ Code = 1026,Message = "Cart item already exists.", HttpStatusCode = 400 } },
        {ErrorCode.INSUFFICIENT_QUANTITY,new ErrorDetail{ Code = 1027,Message = "Insufficient quantity.", HttpStatusCode = 400 } },
        {ErrorCode.INCORRECT_PERCENT,new ErrorDetail{ Code = 1028,Message = "Percent must be between 0 and 1 .", HttpStatusCode = 400 } },
        { ErrorCode.MAX_VALUE ,new ErrorDetail{ Code = 1029,Message = "Max value must be greater than 0.", HttpStatusCode = 400 } },
        { ErrorCode.MIN_VALUE ,new ErrorDetail{ Code = 1030,Message = "Min value must be greater than 0.", HttpStatusCode = 400 } },
        { ErrorCode.END_DATE ,new ErrorDetail{Code = 1031,Message = "The end date cannot be earlier than today..", HttpStatusCode = 400 } },
        { ErrorCode.START_DATE ,new ErrorDetail{Code=1032, Message = "Start date cannot be earlier than today..", HttpStatusCode = 400 } },
        { ErrorCode.DISCOUNT_EXISTED ,new ErrorDetail{ Code = 1033,Message = "Discount already exists.", HttpStatusCode = 400 } },
        {ErrorCode.ORDER_NOT_FOUND ,new ErrorDetail{ Code = 1034,Message = "There are no orders in the shopping cart.", HttpStatusCode = 400 } },
        {ErrorCode.SHOPPING_NOT_FOUND,new ErrorDetail{ Code = 1035,Message = "There are orders that are not in your shopping cart.", HttpStatusCode = 400 } },
        {ErrorCode.INVALID_DISCOUNT,new  ErrorDetail{ Code = 1036,Message = "Invalid discount.", HttpStatusCode = 400 } },
        { ErrorCode.DISCOUNT_NOT_FOUND ,new ErrorDetail{ Code = 1037,Message = "Discount not found.", HttpStatusCode = 400 } },
    };

    public static ErrorDetail Get(ErrorCode code)
        => _errorDetails.TryGetValue(code, out var detail)
            ? detail
            : new ErrorDetail { Code = 9999, Message = "Unknown error", HttpStatusCode = 500 };
}