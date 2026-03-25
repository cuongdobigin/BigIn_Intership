using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class BulkImageRequest
{
    [Required]
    public int BookId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "INCORRECT_IMAGE")]
    public List<string> Links { get; set; } = new List<string>();
}
