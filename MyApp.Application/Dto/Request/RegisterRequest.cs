using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class RegisterRequest
{
    [Required(ErrorMessage = "INCORRECT_EMAIL")]
    [EmailAddress]
    public string username { get; set; }
    [Required(ErrorMessage = "INCORRECT_PASSWORD")]
    [Length(8,15)]
    public string password { get; set; }
}