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
    [Column("is_active")]
    public bool isActive { get; set; }=true;
    
    public Account Account { get; set; } = null!;
    
    public Book  Books { get; set; } = null!;
}