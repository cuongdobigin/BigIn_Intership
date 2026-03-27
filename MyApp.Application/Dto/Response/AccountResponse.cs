namespace MyApp.Application.Dto.Response;

public class AccountResponse
{
    public int Id { get; set; }
    public string username { get; set; }
    public Boolean isActive { get; set; }
    public UserResponse? User { get; set; }
}