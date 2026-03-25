using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myapp.Domain.Entity;

[Table("review")]
public class Review
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("message")]
    public string Message { get; set; } = "";

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    public Account Account { get; set; } = null!;
    
    public Book Book { get; set; } = null!;
}