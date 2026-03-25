namespace MyApp.Application.Dto.Response;

public class ShoppingCartResponse
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public bool IsActive { get; set; }
    public int BookId { get; set; }
    public string BookName { get; set; } = string.Empty;
    public decimal BookPrice { get; set; }
    public string? BookImage { get; set; }
}
