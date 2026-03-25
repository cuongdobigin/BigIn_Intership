using System.ComponentModel.DataAnnotations;
using Myapp.Domain.Exceptions;

namespace MyApp.Application.Dto.Request;

public class ShoppingCartRequest
{
    [Required] public int BookId { get; set; }

    [Required]
    [Range(1, int.MaxValue,  ErrorMessage = "INCORRECT_QUANTITY")]
    public int Amount { get; set; }
}
