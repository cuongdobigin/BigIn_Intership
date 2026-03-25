using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class ChangePasswordRequest
{
    public string username { get; set; } 
    public string oldPassword { get; set; } 
    [Required(ErrorMessage = "INCORRECT_PASSWORD")]
    [Length(8,15)] 
    public string newPassword { get; set; } 
    
}
