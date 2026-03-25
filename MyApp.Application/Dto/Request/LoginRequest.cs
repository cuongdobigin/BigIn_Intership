using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class LoginRequest
{
    public string Username { get; set; } 
    public string Password { get; set; } 
}