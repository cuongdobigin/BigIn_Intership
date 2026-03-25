using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myapp.Domain.Entity;

[Table("Payment")]
public class Payment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("request_id")]
    public string RequestId { get; set; } = string.Empty;

    [Column("trans_id")]
    public long TransId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;
}