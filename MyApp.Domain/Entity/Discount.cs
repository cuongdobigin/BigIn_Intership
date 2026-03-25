// Discount.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myapp.Domain.Entity;

[Table("Discount")]
public class Discount
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("min_apply")]
    public decimal MinApply { get; set; }

    [Column("max_apply")]
    public decimal MaxApply { get; set; }

    [Column("is_active")] public bool IsActive { get; set; } = true;

    [Column("percent")]
    public decimal Percent { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}