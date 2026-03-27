namespace MyApp.Application.Dto.Request;

public class SearchFilterDto
{
    public List<string> Keywords { get; set; } = new();
    public string? TypeBook { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
}