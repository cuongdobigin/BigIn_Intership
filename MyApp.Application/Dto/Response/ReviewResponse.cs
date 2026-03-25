namespace MyApp.Application.Dto.Response;

public class ReviewResponse
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string AccountUsername { get; set; } = string.Empty;
    public int BookId { get; set; }
}
