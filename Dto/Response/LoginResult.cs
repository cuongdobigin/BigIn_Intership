namespace webApi.Dto.Response;

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool isFirstTime { get; set; }
}
