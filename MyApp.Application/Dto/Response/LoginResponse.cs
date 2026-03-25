namespace MyApp.Application.Dto.Response;


public class LoginResponse
{
    public string token { get; set; }
    public bool isFirstTime { get; set; }
}