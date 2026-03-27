namespace MyApp.Application.Dto.Response;


public class BookResponse
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public decimal Price { get; set; }
    
    public string Description { get; set; } 
    
    public bool Stock { get; set; }
    
    public int quantity { get; set; }
    public string Author { get; set; }
    public bool isActive { get; set; }
    public int TypeBookId { get; set; }
    public ICollection<ImageResponse> Images { get; set; } = new List<ImageResponse>();
}