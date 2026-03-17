using System.ComponentModel.DataAnnotations;

namespace webApi.Dto.Request;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    string username;
    string password;
}