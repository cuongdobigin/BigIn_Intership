using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Myapp.Domain.Entity;

[Table("Book")]
public class Book
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("price")]
    public decimal Price { get; set; }

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("stock")]
    public bool Stock { get; set; }
    
    [Column("author")]
    public string Author { get; set; } = "";
    
    [Column("quantity")]
    public int quantity {get; set;} = 0;
    
    [Column("is_active")] public bool IsActive { get; set; } = true;
    
    [Column("type_book_Id")]
    public int type_book_Id { get; set; }
    public TypeBook TypeBook { get; set; } = null!;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}