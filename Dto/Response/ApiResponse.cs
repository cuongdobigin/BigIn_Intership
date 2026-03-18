namespace webApi.Dto.Response;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Code { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Success", int code = 200)
        => new() { IsSuccess = true, Data = data, Message = message, Code = code };

    public static ApiResponse<T> Fail(string message, int code = 400)
        => new() { IsSuccess = false, Message = message, Code = code };

    public static ApiResponse<object> Success(string message = "Success", int code = 200)
        => new() { IsSuccess = true, Message = message, Code = code };
}