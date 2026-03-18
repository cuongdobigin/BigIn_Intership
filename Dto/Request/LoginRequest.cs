using System.ComponentModel.DataAnnotations;

namespace webApi.Dto.Request;

public class LoginRequest
{
    public string Username { get; set; } 
    public string Password { get; set; } 
}