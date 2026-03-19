namespace webApi.Dto.Response;

public class LoginResponse
{
    public string token { get; set; }
    public Boolean isFirstTime { get; set; }
}