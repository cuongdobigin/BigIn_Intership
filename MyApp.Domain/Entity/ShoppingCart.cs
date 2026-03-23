using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myapp.Domain.Entity;

[Table("ShoppingCart")]
public class ShoppingCart
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("amount")]
    public int Amount { get; set; }
    
    public Account Account { get; set; } = null!;
    
    public ICollection<Book>  Books { get; set; } = new List<Book>();
}