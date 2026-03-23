using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myapp.Domain.Entity;

[Table("DetailOrder")]
public class DetailOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public decimal Price { get; set; }
    
    public Order Order { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = new List<Book>();
}