
using System.ComponentModel.DataAnnotations;

namespace MyApp.Application.Dto.Request;

public class BookRequest
{
    
    public string Name { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "INCORRECT_PRICE")]
    public decimal Price { get; set; }
    
    public string Description { get; set; } 
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "INCORRECT_QUANTITY")]
    public int quantity { get; set; }
    
    
    public string Author { get; set; }
    
    public int TypeBookId { get; set; }
}