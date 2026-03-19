using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApi.Domain.Entity;

[Table("TypeBook")]
public class TypeBook
{
    [Key]
    [Column("type_id")]
    public int TypeId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("description")]
    public string Description { get; set; } = "";

    [Column("is_active")]
    public bool IsActive { get; set; }
    
    public ICollection<Book> Books { get; set; } = new List<Book>();
}