using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace webApi.Domain.Entity;

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

    [Column("available")]
    public bool Available { get; set; }
    
    public int TypeBookId { get; set; }
    public TypeBook TypeBook { get; set; } = null!;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}