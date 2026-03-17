using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApi.Domain.Entity;
[Table("Account")]
public class Account
{
    [Column("Id")]
    public int Id { get; set; }
    [Column("username")]
    public string username { get; set; }
    [Column("password")]
    public string password { get; set; }
    [Column("is_active")]
    public Boolean isActive { get; set; }
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
    [Column("updatedAt")]
    public DateTime UpdatedAt { get; set; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public User? User { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ShoppingCart ShoppingCart { get; set; }
    
}