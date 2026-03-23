using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class ReviewRequest
{
    [Required]
    public string Message { get; set; } = string.Empty;
    
}
