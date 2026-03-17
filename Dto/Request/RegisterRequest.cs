using System.ComponentModel.DataAnnotations;

namespace webApi.Dto.Request;

public class RegisterRequest
{
    public string Username { get; set; } 
    public string Password { get; set; } 
}