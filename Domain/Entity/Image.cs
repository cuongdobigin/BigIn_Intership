using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApi.Domain.Entity;

[Table("Image")]
public class Image
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("link")]
    public string Link { get; set; } = "";
    
    public Book Book { get; set; } = null!;
}