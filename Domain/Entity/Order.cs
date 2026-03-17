using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApi.Domain.Entity;

[Table("Order")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("tax")]
    public decimal Tax { get; set; }

    [Column("sub_total")]
    public decimal SubTotal { get; set; }

    [Column("payment_total")]
    public decimal PaymentTotal { get; set; }

    [Column("payment_status")]
    public string PaymentStatus { get; set; } = "";
    
    public Account Account { get; set; } = null!;
    
    public Discount? Discount { get; set; }
    public ICollection<DetailOrder> DetailOrder { get; set; } = new List<DetailOrder>();
    
}